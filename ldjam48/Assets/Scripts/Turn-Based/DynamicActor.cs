//
//
// A Dynamic Actor is an actor that can move, has stats, and can die
//
//

using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(ActorHealthComponent))]
public class DynamicActor : Actor {

    public Stats ActorStats;
    private ActorHealthComponent m_Health;

    [Header("Handling Collisions")]
    public LayerMask CollideWith;
    public LayerMask TriggerLayer;

    [Header("Sound Effects")]
    public AudioClip[] ActorMovedSounds;
    public AudioClip[] MovementDeniedSounds;
    public AudioClip[] ActorAttackedSounds;
    public AudioClip[] ActorDamagedSounds;
    public AudioClip[] ActorDiedSounds;
    public AudioClip[] HealthRegenSounds;

    [Header("Particle Effects")]
    public ParticleSystem DamagedParticle;
    public ParticleSystem MovementParticle;

    // Variables to Handle Sprite Flashing
    private const float FLASH_DELAY_TIME = 0.1f;
    private WaitForSeconds m_FlashDelayWait;
    private SpriteRenderer m_ActorSpriteRenderer;

    // Variables to Handle Movement
    private bool m_IsActorCurrentlyMoving = false;

    // Events and Delegates ----------------------------
    public event System.Action OnActorMoved;
    public event System.Action OnActorMoveDenied;
    public event System.Action OnActorAttacked;
    public event System.Action OnActorWasHit;
    public event System.Action OnActorHealed;
    public event System.Action OnActorDied;
    public event System.Action OnActorInteracted;
    public TurnDelegate TurnDelegate;
    // -------------------------------------------------

    // TIMING ------------------------------------------
    private const float ACTION_TIME = 0.1f;
    private const float MOVEMENT_TIME = 0.1f;
    private const float INTERACTION_TIME = 0.1f;
    // -------------------------------------------------

    private void Start() {
        m_Health = GetComponent<ActorHealthComponent>();
        m_ActorSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        m_FlashDelayWait = new WaitForSeconds(FLASH_DELAY_TIME);
    }

    // ========================================================
    //
    // Overriding actor functions
    //
    // ========================================================
    public override void InitializeActor(EActorType _ActorType, string _ActorName) {
        base.InitializeActor(_ActorType, _ActorName);

        if (ActorStats == null) {
            Debug.LogError($"{this.name} doesn't have Actor Stats assigned!");
        }
    }

    public override bool TakeTurn() {
        if (TurnDelegate != null) {
            return TurnDelegate();
        }

        Debug.LogWarning($"No function to handle turn was assigned on {this.name} ");
        return true;
    }

    // ==========================================================================================
    //
    // Movement
    //
    // ==========================================================================================
    #region Movement
    /// <summary>
    /// <para>Tries to move the Dynamic Actor on the specified direction</para>
    /// </summary>
    /// <param name="_MovementDirection">Direction which agent will attempt to move</param>
    public bool Move(Vector2 _MovementDirection) {
        bool bWillEngageInCombat = WillEngageOnCombatOnMovement(_MovementDirection);
        bool bCanMoveOnDirection = false;

        if (!bWillEngageInCombat) {
            bCanMoveOnDirection = CanMoveOnDirection(_MovementDirection);
        }

        return Move(_MovementDirection, bCanMoveOnDirection, bWillEngageInCombat);
    }

    private bool WillEngageOnCombatOnMovement(Vector2 _MovementDirection) {
        if (_MovementDirection == Vector2.zero) {
            return false;
        }

        Vector2 PositionToCheck = m_CurrentPosition + _MovementDirection;
        Actor ActorToInteract = TurnBasedManager.s_Instance.WhatActorIsAt(PositionToCheck);

        if (ActorToInteract != null) {
            if (ActorToInteract is DynamicActor && ActorToInteract.ActorType != m_ActorType) {

                // need to get the attacker roll type!
                Weapon.EWeaponRoll RollType = Weapon.EWeaponRoll.R1d4;
                if(ActorType == EActorType.EAT_Player) {
                    RollType = ResourceLocator.instance.PlayerWeapon.RollType;
                } else if(ActorType == EActorType.EAT_Enemy) {
                    RollType = GetComponent<Enemy>().MonsterStat.DamageRoll;
                }

                TurnBasedManager.s_Instance.HandleCombat(this, (DynamicActor)ActorToInteract, RollType);
                return true;
            }
        }

        return false;
    }

    protected bool CanMoveOnDirection(Vector2 _MovementDirection) {
        if (_MovementDirection == Vector2.zero) {
            return false;
        }

        Actor ActorOnPosition = TurnBasedManager.s_Instance.WhatActorIsAt(m_CurrentPosition + _MovementDirection);

        if (ActorOnPosition != null && ActorOnPosition.ActorType == m_ActorType) {
            return false;
        }

        bool HasCollision = CheckIfHasCollision(m_CurrentPosition + _MovementDirection);
        Collider2D TriggerCollision = CheckIfHasTriggerCollision(m_CurrentPosition + _MovementDirection);

        if (TriggerCollision) {
            // IInteractable Interactable = TriggerCollision.GetComponent<IInteractable>();
            // Interactable?.Interact();
            OnActorInteracted?.Invoke();
        }


        return !HasCollision;
    }

    public bool IsMovementLegal(Vector2 _MovementDirection) {
        Actor ActorOnPosition = TurnBasedManager.s_Instance.WhatActorIsAt(m_CurrentPosition + _MovementDirection);

        if(ActorOnPosition != null && ActorOnPosition.ActorType == m_ActorType) {
            return false;
        }

        bool HasCollision = CheckIfHasCollision(m_CurrentPosition + _MovementDirection);
        return !HasCollision;
    }

    protected bool CheckIfHasCollision(Vector2 _Position) {
        Collider2D BlockedCollision = Physics2D.OverlapCircle(_Position, 0.05f, CollideWith);
        return (BlockedCollision != null);
    }

    protected Collider2D CheckIfHasTriggerCollision(Vector2 _Position) {
        return Physics2D.OverlapCircle(_Position, 0.05f, TriggerLayer);
    }
    #endregion

    // ==========================================================================================
    //
    // Actually Moving
    //
    // ==========================================================================================
    #region HANDLING ACTUAL MOVEMENT
    private bool Move(Vector2 _MovementDirection, bool _bCanMove, bool _bHasActed) {
        if (m_IsActorCurrentlyMoving) {
            return false;
        }

        if (_bHasActed) {
            ActorActed(_MovementDirection);
            OnActorAttacked?.Invoke();
            return true;
        } else if (_bCanMove) {
            ActorMoved(_MovementDirection);
            OnActorMoved?.Invoke();
            return true;
        } else {
            ActorMovementDenied(_MovementDirection);
            OnActorMoveDenied?.Invoke();
            return false;
        }
    }

    private void ActorActed(Vector2 _DirectionWhichActed) {
        InitializeMovementAndAction(_DirectionWhichActed);

        Sequence ActionSequence = DOTween.Sequence();
        ActionSequence.Append(transform.DOMove(CurrentPosition + _DirectionWhichActed, ACTION_TIME / 2.0f).SetEase(Ease.InOutQuint));
        ActionSequence.AppendInterval(0.1f);
        ActionSequence.Append(transform.DOMove(CurrentPosition, ACTION_TIME / 2.0f).SetEase(Ease.InOutQuint));
        ActionSequence.onComplete += MovementRoutineFinished;
        ActionSequence.Play();
    }

    private void ActorMoved(Vector2 _MovementDirection) {
        InitializeMovementAndAction(_MovementDirection);

        Vector2 MidwayPoint = m_CurrentPosition + new Vector2(_MovementDirection.x / 2, _MovementDirection.y / 2 + 0.25f);
        Vector2 DestinationPoint = m_CurrentPosition + _MovementDirection;
        m_CurrentPosition = DestinationPoint;

        Sequence MovementSequence = DOTween.Sequence();
        MovementSequence.Append(transform.DOMove(MidwayPoint, MOVEMENT_TIME / 2.0f).SetEase(Ease.InOutQuint));
        MovementSequence.Append(transform.DOMove(DestinationPoint, MOVEMENT_TIME / 2.0f).SetEase(Ease.OutBack));
        MovementSequence.onComplete += MovementRoutineFinished;
        MovementSequence.Play();
    }

    private void ActorInteracted(Vector2 _InteractionDirection) {
        InitializeMovementAndAction(_InteractionDirection);

        Sequence InteractionSequence = DOTween.Sequence();
        InteractionSequence.Append(transform.DOMove(m_CurrentPosition + _InteractionDirection, INTERACTION_TIME / 2.0f).SetEase(Ease.InOutExpo));
        InteractionSequence.AppendInterval(0.1f);
        InteractionSequence.Append(transform.DOMove(m_CurrentPosition, INTERACTION_TIME / 2.0f).SetEase(Ease.InOutExpo));
        InteractionSequence.onComplete += MovementRoutineFinished;
        InteractionSequence.Play();
    }

    private void ActorMovementDenied(Vector2 _MovementDirection) {
        InitializeMovementAndAction(_MovementDirection);

        Sequence TriedMovementSequence = DOTween.Sequence();
        TriedMovementSequence.Append(transform.DOMove(m_CurrentPosition + new Vector2(0.0f, 0.25f), MOVEMENT_TIME / 2.0f).SetEase(Ease.InOutQuint));
        TriedMovementSequence.Append(transform.DOMove(m_CurrentPosition, MOVEMENT_TIME / 2.0f).SetEase(Ease.OutBack));
        TriedMovementSequence.onComplete += MovementRoutineFinished;
        TriedMovementSequence.Play();
    }

    private void InitializeMovementAndAction(Vector2 _MovementDirection) {
        m_IsActorCurrentlyMoving = true;

        if (_MovementDirection.x != 0) {
            transform.localScale = new Vector3(Mathf.Sign(_MovementDirection.x) * Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void MovementRoutineFinished() {
        m_IsActorCurrentlyMoving = false;
        transform.position = m_CurrentPosition;
    }
    #endregion

    // ==========================================================================================
    //
    // COMBAT
    //
    // ==========================================================================================
    #region COMBAT
    public virtual void DealtDamage() {
        // TODO
    }

    public virtual void Heal(int _Amount) {
        m_Health.Heal(_Amount);
        OnActorHealed?.Invoke();
    }

    public virtual void SufferedDamage(int _Damage) {
        m_Health.TakeDamage(_Damage);
        OnActorWasHit?.Invoke();

        if (m_Health.IsDead) {
            Die();
            return;
        }

    }

    protected virtual void Die() {
        OnActorDied?.Invoke();
        TurnBasedManager.s_Instance.RemoveActor(this);
        Destroy(gameObject);
    }
    #endregion

}