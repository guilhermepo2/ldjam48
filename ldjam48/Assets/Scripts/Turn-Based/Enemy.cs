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
        m_Actor.InitializeActor(EActorType.EAT_Enemy, MonsterStat.MonsterName);
        GetComponent<ActorHealthComponent>().SetMaxHealth(MonsterStat.HitPoints);
    }

    private void Start() {
        m_HeroReference = FindObjectOfType<Hero>();
        m_Actor.TurnDelegate = TakeTurn;
        m_Actor.OnActorWasHit += TakeDamage;
        m_Actor.OnActorDied += EnemyDie;

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
                Debug.Log("Enemy has no legal movements...");
                return true;
            }

            Vector2 ChosenMovement = Vector2.zero;
            Vector2 HeroPosition = m_HeroReference.GetComponent<DynamicActor>().CurrentPosition;

            foreach(Vector2 movement in LegalMovements) {
                // making AI less dumb, but not too smart
                if(m_Actor.CurrentPosition + movement == HeroPosition) {
                    // adding a 67% chance of miss just because!
                    if(Random.value < 0.33f) {
                        ChosenMovement = movement;
                    }
                    break;
                }

                int DistanceChosenMovement = MathUtilities.ManhattanDistance(m_Actor.CurrentPosition + ChosenMovement, HeroPosition);
                int DistanceCurrentMovement = MathUtilities.ManhattanDistance(m_Actor.CurrentPosition + movement, HeroPosition);

                if(
                    DistanceCurrentMovement <
                    DistanceChosenMovement
                    ) {
                    ChosenMovement = movement;
                }
            }

            m_Actor.Move(ChosenMovement);

        }

        return true;
    }

    private void EnemyDie() {
        if(MonsterStat.FoodToDrop.Length > 0) {
            if(Random.value < 0.25f) { // giving a 20% chance flat to drop food
                Food drop = MonsterStat.FoodToDrop.RandomOrDefault();
                if(drop != null) {
                    FoodDrop dropObject = Instantiate(ResourceLocator.instance.FoodDropPrefab, m_Actor.CurrentPosition, Quaternion.identity).GetComponent<FoodDrop>();
                    dropObject.foodObject = drop;
                    dropObject.ApplyFood();
                }
            }
        }
    }
}