using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DisableSpriteOnStart : MonoBehaviour {
    void Start() {
        Color newColor = GetComponent<Tilemap>().color;
        newColor.a = 0.0f;
        GetComponent<Tilemap>().color = newColor;
    }
}
