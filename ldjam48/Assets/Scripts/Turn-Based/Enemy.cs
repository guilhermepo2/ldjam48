using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DynamicActor))]
public class Enemy : MonoBehaviour {
    private DynamicActor m_Actor;
    private Hero m_HeroReference;

    public MonsterStats MonsterStat;

    private void Awake() {
        m_Actor = GetComponent<DynamicActor>();
        m_Actor.InitializeActor(EActorType.EAT_Enemy);
        GetComponent<ActorHealthComponent>().SetMaxHealth(MonsterStat.HitPoints);
    }

    private void Start() {
        m_HeroReference = FindObjectOfType<Hero>();
        m_Actor.TurnDelegate = TakeTurn;
        m_Actor.OnActorWasHit += TakeDamage;

        Debug.Log($"Adding enemy {name} to actor list");
        TurnBasedManager.s_Instance.AddActor(m_Actor);
    }

    private void TakeDamage() {
        Debug.Log($"{this.name} took damage!");
    }

    private bool TakeTurn() {
        
        if(m_HeroReference != null) {
            Vector2[] PossibleMovements = { Vector2.left, Vector2.up, Vector2.right, Vector2.down };
            List<Vector2> LegalMovements = new List<Vector2>();

            foreach(Vector2 movement in PossibleMovements) {
                if(m_Actor.IsMovementLegal(movement)) {
                    LegalMovements.Add(movement);
                }
            }

            if(LegalMovements.Count == 0) {
                // ?
                return true;
            }

            Vector2 ChosenMovement = Vector2.zero;
            foreach(Vector2 movement in LegalMovements) {
                if(
                    MathUtilities.ManhattanDistance(m_Actor.CurrentPosition + movement, m_HeroReference.transform.position) <
                    MathUtilities.ManhattanDistance(m_Actor.CurrentPosition + ChosenMovement, m_HeroReference.transform.position)
                    ) {
                    ChosenMovement = movement;
                }
            }

            m_Actor.Move(ChosenMovement);

        }

        return true;
    }
}