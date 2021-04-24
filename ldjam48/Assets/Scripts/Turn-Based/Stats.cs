using UnityEngine;

[CreateAssetMenu(fileName = "Actor Stats", menuName = "Turn Based/Actor Stats")]
public class Stats : ScriptableObject {
    public int MaxHealth;
    public int BaseDamage;

    // TODO: Loot Drop?
}