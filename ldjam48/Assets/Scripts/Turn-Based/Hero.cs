using System;
using UnityEngine;

public class Hero : MonoBehaviour {

    private BaseInput m_PlayerInput;
    private EMovementDirection m_CurrentMovementDirection;
    private DynamicActor m_ActorReference;

    private bool m_IsInputBlocked = false;
    public bool IsInputBlocked {
        get { return m_IsInputBlocked; }
        set { m_IsInputBlocked = value; }
    }

    private void Awake() {
        m_ActorReference = GetComponent<DynamicActor>();

        if (m_ActorReference != null) {
            InitializeHero();
            m_ActorReference.TurnDelegate = TakeTurn;
            m_ActorReference.OnActorDied += Die;
        }
    }

    private void Start() {
        TurnBasedManager.s_Instance.AddActor(m_ActorReference);
    }

    private void InitializeHero() {
        m_PlayerInput = new AxisInput();
        m_ActorReference.InitializeActor(EActorType.EAT_Player);
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