using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLocator : MonoBehaviour {
    public static ResourceLocator instance;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    // TODO
    // Inventory
    // Save
    // Info on the Dungeon Generation
    // Player Info
    // basically everything that has to be persitent between play sessions and scenes should go here!
}
