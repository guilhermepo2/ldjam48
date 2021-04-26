using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityUIManager : MonoBehaviour {
    public static CityUIManager instance;

    public GameObject GoToDungeon;
    public GameObject ProgressUI;

    public Food CheeseObject;
    public Food SmallSteakObject;
    public Text YourGoldText;

    [Header("Stats")]
    public Text CurrentStrength;
    public Text CurrentConstitution;
    public Text PriceToLevel;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        GoToDungeon.SetActive(false);
        ProgressUI.SetActive(false);
    }

    public void ShowGoToDungeon() {
        GoToDungeon.SetActive(true);
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorMoved += HideGoToDungeon;
    }

    public void ShowProgressUI() {
        ProgressUI.SetActive(true);
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorMoved += HideProgressUI;
        UpdateGoldCount();
        UpdateStats();
    }

    public void HideProgressUI() {
        ProgressUI.SetActive(false);
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorMoved -= HideProgressUI;
    }

    private void UpdateGoldCount() {
        YourGoldText.text = $"{ResourceLocator.instance.GoldCount}";
    }

    public void HideGoToDungeon() {
        GoToDungeon.SetActive(false);
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorMoved -= HideGoToDungeon;
    }

    public void ChangeToDungeonScene() {
        ResourceLocator.instance.GoToDungeonScene();
    }
    
    // -------------------------------------------------------------------------
    // Progress UI
    // -------------------------------------------------------------------------

    public void UpdateStats() {
        CurrentStrength.text = $"{ResourceLocator.instance.PlayerExtraStr}";
        CurrentConstitution.text = $"{ResourceLocator.instance.PlayerExtraCon}";
        PriceToLevel.text = $"{ResourceLocator.instance.CurrentLevel * 5}";
    }

    public void AddStr() {
        int Price = ResourceLocator.instance.CurrentLevel * 5;

        if(ResourceLocator.instance.GoldCount > Price) {
            ResourceLocator.instance.IncrementStr();
            ResourceLocator.instance.RemoveGold(Price);
            UpdateStats();
            UpdateGoldCount();
        }
    }

    public void AddCon() {
        int Price = ResourceLocator.instance.CurrentLevel * 5;

        if (ResourceLocator.instance.GoldCount > Price) {
            ResourceLocator.instance.IncrementCon();
            ResourceLocator.instance.RemoveGold(Price);
            UpdateStats();
            UpdateGoldCount();
        }
    }

    public void BuyPotion() {
        int PotionPrice = 15;

        if(ResourceLocator.instance.GoldCount > PotionPrice) {
            ResourceLocator.instance.PlayerGotResource(ResourceDrop.EResource.Potion, 1);
            ResourceLocator.instance.RemoveGold(PotionPrice);
        }

        UpdateGoldCount();
    }

    public void BuyCheese() {
        int CheesePrice = 8;

        if (ResourceLocator.instance.GoldCount > CheesePrice) {
            ResourceLocator.instance.PlayerPickedUpFood(CheeseObject);
            ResourceLocator.instance.RemoveGold(CheesePrice);
        }

        UpdateGoldCount();
    }

    public void BuySmallSteak() {
        int SmallSteakPrice = 10;

        if (ResourceLocator.instance.GoldCount > SmallSteakPrice) {
            ResourceLocator.instance.PlayerPickedUpFood(SmallSteakObject);
            ResourceLocator.instance.RemoveGold(SmallSteakPrice);
        }

        UpdateGoldCount();
    }
}
