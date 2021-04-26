using System;
using UnityEngine;

[RequireComponent(typeof(DynamicActor))]
public class Hero : MonoBehaviour {

    public event System.Action OnHeroInitialized;

    private BaseInput m_PlayerInput;
    private EMovementDirection m_CurrentMovementDirection;
    private DynamicActor m_ActorReference;

    // Food Related...
    private const int m_MaxFoodTicks = 30;
    private int m_CurrentFoodTicks = 0;
    private const int m_HealEveryNTurns = 3;
    private int m_FoodHealingTurnCounter = 0;

    private bool m_bIsInitialized;

    private bool m_IsInputBlocked = false;
    public bool IsInputBlocked {
        get { return m_IsInputBlocked; }
        set { m_IsInputBlocked = value; }
    }

    private void Awake() {
        m_bIsInitialized = false;
    }

    private void Start() {
        m_PlayerInput = new AxisInput();
    }

    public void InitializeHero() {
        
        if(m_bIsInitialized) {
            return;
        }

        m_ActorReference = GetComponent<DynamicActor>();

        m_ActorReference.TurnDelegate = TakeTurn;
        m_ActorReference.OnActorDied += Die;
        m_ActorReference.OnActorMoved += UpdateVisibility;
        m_ActorReference.OnActorMoved += TickFood;
        m_ActorReference.OnActorInteractedWith += OnInteractedWith;
        TurnBasedManager.s_Instance.AddActor(m_ActorReference);

        m_ActorReference.InitializeActor(EActorType.EAT_Player, "Hero");
        GetComponent<ActorHealthComponent>().SetMaxHealth(m_ActorReference.ActorStats.Constitution);
        OnHeroInitialized?.Invoke();
        m_bIsInitialized = true;
    }
    
    public void UpdateVisibility() {
        GetComponentInChildren<FourthDimension.Roguelike.FieldOfView>().RefreshVisibility(transform.position);
    }

    public void UpdatePosition(Vector2 _position) {
        this.transform.position = _position;
        m_ActorReference.CurrentPosition = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
    }

    private void TickFood() {
        m_FoodHealingTurnCounter++;

        if(m_FoodHealingTurnCounter % m_HealEveryNTurns == 0) {
            if(m_CurrentFoodTicks > 0) {
                m_CurrentFoodTicks -= 1;
                m_ActorReference.Heal(1);
            }

            m_FoodHealingTurnCounter = 0;
        }
    }

    public void AddFood(int _Amount) {
        m_CurrentFoodTicks = Mathf.Min(m_CurrentFoodTicks + _Amount, m_MaxFoodTicks);
    }

    private void Update() {
        m_CurrentMovementDirection = m_PlayerInput.TickInput();
    }

    private bool TakeTurn() {
        if (m_CurrentMovementDirection == EMovementDirection.EMD_NONE || m_IsInputBlocked) {
            return false;
        }

        return m_ActorReference.Move(InputUtilities.GetMovementVectorFromDirection(m_CurrentMovementDirection));
    }

    private void Die() {

    }

    private void OnInteractedWith(Collider2D other) {
        if(other.name.Contains("Downstairs")) { // this is bad lol
            DungeonManager.instance.GoDeeper();
        } else if(other.GetComponent<FoodDrop>() != null) {
            FoodDrop whatIGot = other.GetComponent<FoodDrop>();
            ResourceLocator.instance.PlayerPickedUpFood(whatIGot.foodObject);
            Destroy(other.gameObject);
        } else if(other.GetComponent<ResourceDrop>()) {
            ResourceDrop r = other.GetComponent<ResourceDrop>();
            ResourceLocator.instance.PlayerGotResource(r.ResourceType, r.Amount);
            Destroy(other.gameObject);
        }
    }
}