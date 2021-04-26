using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityUIManager : MonoBehaviour {
    public static CityUIManager instance;

    public GameObject GoToDungeon;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        HideGoToDungeon();
    }

    public void ShowGoToDungeon() {
        GoToDungeon.SetActive(true);
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorMoved += HideGoToDungeon;
    }

    public void HideGoToDungeon() {
        GoToDungeon.SetActive(false);
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorMoved -= HideGoToDungeon;
    }

    public void ChangeToDungeonScene() {
        ResourceLocator.instance.GoToDungeonScene();
    }
}
