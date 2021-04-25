//
//
// An actor is anything that can take a turn
//
//


using UnityEngine;

public enum EActorType {
    EAT_Player = 1,
    EAT_Enemy = 10,
    EAT_Environment = 20,
    EAT_MAX = 999
}

public abstract class Actor : MonoBehaviour {
    protected string m_ActorName;
    public string ActorName {
        get {
            return m_ActorName;
        }
    }

    protected Vector2 m_CurrentPosition;
    public Vector2 CurrentPosition {
        get { return this.m_CurrentPosition; }
        set { m_CurrentPosition = value; }
    }

    protected EActorType m_ActorType;
    public EActorType ActorType {
        get { return this.m_ActorType; }
        set { m_ActorType = value; }
    }

    public virtual void InitializeActor(EActorType _ActorType, string ActorName) {
        m_CurrentPosition = new Vector2(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y));
        m_ActorType = _ActorType;
        m_ActorName = ActorName;
    }

    /// <summary>
    /// Called from the Turn Based Manager - Grants the actor a turn.
    /// </summary>
    /// <returns>Whether this character already took a turn</returns>
    public abstract bool TakeTurn();
}