using UnityEngine;

[CreateAssetMenu(fileName = "Actor Stats", menuName = "Turn Based/Actor Stats")]
public class Stats : ScriptableObject {
    public int Strength;
    public int Dexterity;
    public int Constitution;
    public int Intelligence;
    public int Wisdom;
    public int Charisma;
}