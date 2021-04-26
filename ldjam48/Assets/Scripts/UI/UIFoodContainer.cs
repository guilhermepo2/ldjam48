using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFoodContainer : MonoBehaviour {
    public Food foodObject;

    private Image m_ImageReference;
    private Text m_FoodCount;

    private void Start() {
        m_ImageReference = transform.GetChild(0).GetComponent<Image>();
        m_FoodCount = GetComponentInChildren<Text>();
        Apply();
    }

    public void Apply() {
        m_ImageReference.sprite = foodObject.sprite;
        m_FoodCount.text = $"{ResourceLocator.instance.GetFoodCountFromFoodObject(foodObject)}";
    }

    public void Eat() {
        ResourceLocator.instance.PlayerWantsToEat(foodObject);
    }
}
