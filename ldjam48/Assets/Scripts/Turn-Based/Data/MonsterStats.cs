using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster Stats", menuName = "Turn Based/Monster Stats")]
public class MonsterStats : ScriptableObject {
    public string MonsterName;

    public int ArmorClass;
    public int HitPoints;
    public int Speed;

    public int Challenge;
    public int Experience;

    public Weapon.EWeaponRoll DamageRoll;
}
