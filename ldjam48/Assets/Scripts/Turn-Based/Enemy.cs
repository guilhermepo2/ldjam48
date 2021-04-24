using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    private DynamicActor m_Actor;

    private void Awake() {
        m_Actor = GetComponent<DynamicActor>();
        m_Actor.InitializeActor(EActorType.EAT_Enemy);
    }

    private void Start() {
        m_Actor.TurnDelegate = TakeTurn;
        m_Actor.OnActorWasHit += TakeDamage;
        TurnBasedManager.s_Instance.AddActor(m_Actor);
    }

    private void TakeDamage() {
        Debug.Log($"{this.name} took damage!");
    }

    private bool TakeTurn() {
        return true;
    }
}