using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ResourceLocator : MonoBehaviour {
    public static ResourceLocator instance;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }

        m_AllLevels             = new List<string>();
        m_AvailableLevels       = new List<string>();
        m_AlreadyPlayedLevels   = new List<string>();
    }

    [Header("Monster Database")]
    public GameObject[] MonsterPrefabs;

    [Header("Weapon Database")]
    public Weapon[] Weapons;

    [Header("Dungeon Settings")]
    public int CurrentDifficulty;
    private List<string> m_AllLevels;
    private List<string> m_AvailableLevels;
    private List<string> m_AlreadyPlayedLevels;

    [Header("Player Equipments")]
    public Weapon PlayerWeapon;

    private void Start() {
        foreach(string file in System.IO.Directory.GetFiles($"{Application.dataPath}/Levels/")) {
            if(!file.Contains(".meta")) {
                m_AllLevels.Add(file);
            }
        }
    }

    // TODO
    // Inventory
    // Save
    // Info on the Dungeon Generation
    // Player Info
    // basically everything that has to be persitent between play sessions and scenes should go here!
}
