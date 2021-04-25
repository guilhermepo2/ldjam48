using System;
using UnityEngine;

[RequireComponent(typeof(DynamicActor))]
public class Hero : MonoBehaviour {

    private BaseInput m_PlayerInput;
    private EMovementDirection m_CurrentMovementDirection;
    private DynamicActor m_ActorReference;

    private bool m_IsInputBlocked = false;
    public bool IsInputBlocked {
        get { return m_IsInputBlocked; }
        set { m_IsInputBlocked = value; }
    }

    private void Start() {
        m_PlayerInput = new AxisInput();
    }

    public void InitializeHero() {
        m_ActorReference = GetComponent<DynamicActor>();

        m_ActorReference.TurnDelegate = TakeTurn;
        m_ActorReference.OnActorDied += Die;
        m_ActorReference.OnActorMoved += UpdateVisibility;
        TurnBasedManager.s_Instance.AddActor(m_ActorReference);

        m_ActorReference.InitializeActor(EActorType.EAT_Player, "Hero");
        GetComponent<ActorHealthComponent>().SetMaxHealth(m_ActorReference.ActorStats.Constitution);
    }
    
    public void UpdateVisibility() {
        GetComponentInChildren<FourthDimension.Roguelike.FieldOfView>().RefreshVisibility(transform.position);
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
}