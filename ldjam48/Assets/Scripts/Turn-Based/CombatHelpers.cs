using UnityEngine;

public static class CombatHelpers {
    public static int[] ModifierLookupTable = { -5, -4, -4, -3, -3, -2, -2, -1, -1, 0, 0, 1, 1, 2, 2, 3, 3, 4, 4, 5, 5, 6, 6, 7, 7, 8, 8, 9, 9, 10 };

    public static int CalculateModifier(int Score) {
        if (Score > 30) {
            Score = 30;
        }

        if (Score < 1) {
            Score = 1;
        }

        return ModifierLookupTable[Score];
    }

    public static int CalculateDamage(int AttackerModifier, Weapon.EWeaponRoll RollType) {
        int TotalDamage = AttackerModifier;

        switch(RollType) {
            case Weapon.EWeaponRoll.R1d4:
                TotalDamage += Random.Range(1, 4);
                break;
            case Weapon.EWeaponRoll.R1d6:
                TotalDamage += Random.Range(1, 6);
                break;
            case Weapon.EWeaponRoll.R1d8:
                TotalDamage += Random.Range(1, 8);
                break;
            case Weapon.EWeaponRoll.R1d10:
                TotalDamage += Random.Range(1, 10);
                break;
            case Weapon.EWeaponRoll.R1d12:
                TotalDamage += Random.Range(1, 12);
                break;
            case Weapon.EWeaponRoll.R2d6:
                TotalDamage += Random.Range(1, 6);
                TotalDamage += Random.Range(1, 6);
                break;
        }

        return TotalDamage;
    }
}
