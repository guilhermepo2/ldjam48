using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonHUD : MonoBehaviour {

    public static DungeonHUD instance;

    public GameObject GoToCity;
    public GameObject GameOverPanel;

    private void Awake() {
        instance = this;
    }

    public void Start() {
        GoToCity.SetActive(false);
    }

    public void ShowGoToCity() {
        GoToCity.SetActive(true);
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorMoved += HideGoToCity;
    }

    public void HideGoToCity() {
        GoToCity.SetActive(false);
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorMoved -= HideGoToCity;
    }

    public void ClickedGoToCity() {
        ResourceLocator.instance.GoToCityScene();
    }

    public void ClickedGoToMenu() {
        ResourceLocator.instance.GoToMenu();
    }
}
