using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DynamicActor))]
public class Enemy : MonoBehaviour {
    private DynamicActor m_Actor;

    public MonsterStats MonsterStat;

    private void Awake() {
        m_Actor = GetComponent<DynamicActor>();
        m_Actor.InitializeActor(EActorType.EAT_Enemy);
        GetComponent<ActorHealthComponent>().SetMaxHealth(MonsterStat.HitPoints);
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