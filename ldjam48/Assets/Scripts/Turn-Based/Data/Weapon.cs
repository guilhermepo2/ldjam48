using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Turn Based/Weapon")]
public class Weapon : ScriptableObject {
    public enum EWeaponRoll {
        R1d4,
        R1d6,
        R1d8,
        R1d12,
        R2d6,
        R1d10
    }

    public string WeaponName;
    public EWeaponRoll RollType;
    public int Cost;
    public int Weight;
}
