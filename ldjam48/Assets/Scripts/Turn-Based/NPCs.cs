using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class NPCs : MonoBehaviour {
    public enum ENPCType {
        Travel,
        Progress
    }

    public ENPCType NPCType;

    private void Start() {
        
    }
}
