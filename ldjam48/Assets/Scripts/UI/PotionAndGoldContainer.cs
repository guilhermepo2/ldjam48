using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionAndGoldContainer : MonoBehaviour {
    public Text PotionAmount;
    public Text GoldAmount;

    public void Apply() {
        PotionAmount.text = $"{ResourceLocator.instance.PotionCount}";
        GoldAmount.text = $"{ResourceLocator.instance.GoldCount}";
    }

    public void DrinkPotion() {
        ResourceLocator.instance.PlayerWantsToDrinkPotion();
    }
}
