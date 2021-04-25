using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public class PlayerInventory {
    public int SmallHealthPotions;
    public int MediumHealthPotions;
    public int LargeHealthPotions;

    public List<int> FoodCount;

    public PlayerInventory() {
        SmallHealthPotions = 0;
        MediumHealthPotions = 0;
        LargeHealthPotions = 0;
        FoodCount = new List<int>();
    }
}

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
        LoadAllLevels();

        CurrentDifficulty = 0;
        InitializePlayerInventory();
    }

    [Header("Monster Database")]
    public GameObject[] MonsterPrefabs;

    [Header("Weapon Database")]
    public Weapon[] Weapons;

    [Header("Food Database")]
    public Food[] Foods;

    [Header("Dungeon Settings")]
    public int CurrentDifficulty;
    private List<string> m_AllLevels;
    private List<string> m_AvailableLevels;
    private List<string> m_AlreadyPlayedLevels;

    [Header("Player Equipments")]
    public Weapon PlayerWeapon;
    private PlayerInventory m_PlayerInventory;

    public void InitializePlayerInventory() {
        m_PlayerInventory = new PlayerInventory();

        m_PlayerInventory.SmallHealthPotions = 5;

        for (int i = 0; i < Foods.Length; i++) {
            m_PlayerInventory.FoodCount.Add(0);
        }
    }

    private void LoadAllLevels() {
        foreach (string file in System.IO.Directory.GetFiles($"{Application.dataPath}/Levels/")) {
            if (!file.Contains(".meta")) {
                m_AllLevels.Add(file);
                m_AvailableLevels.Add(file);
            }
        }
    }

    public string GetPlayableLevel() {
        if(m_AvailableLevels.Count == 0) {
            foreach(string tLevel in m_AlreadyPlayedLevels) {
                m_AvailableLevels.Add(tLevel);
            }

            m_AlreadyPlayedLevels.Clear();
        }

        string level = m_AvailableLevels.RandomOrDefault();
        m_AvailableLevels.Remove(level);
        m_AlreadyPlayedLevels.Add(level);
        Debug.LogWarning($"Playable Level: {level}");
        return level;
    }

    public void GoDeeper() {
        CurrentDifficulty++;
    }

    // TODO
    // Inventory
    // Save
    // Info on the Dungeon Generation
    // Player Info
    // basically everything that has to be persitent between play sessions and scenes should go here!
}
