using UnityEngine;

[CreateAssetMenu(fileName = "Food", menuName = "Turn Based/Food")]
public class Food : ScriptableObject {
    public string FoodName;
    public Sprite sprite;
    public int CanHealNTimes;
}
