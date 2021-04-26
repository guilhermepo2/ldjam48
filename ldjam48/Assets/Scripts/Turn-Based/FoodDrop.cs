using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FoodDrop : MonoBehaviour {
    public Food foodObject;

    public void ApplyFood() {
        GetComponent<SpriteRenderer>().sprite = foodObject.sprite;
    }
}
