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
        LoadAllLevels();

        CurrentDifficulty = 0;
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
            m_AvailableLevels.Clear();
        }

        string level = m_AvailableLevels.RandomOrDefault();
        m_AvailableLevels.Remove(level);
        m_AlreadyPlayedLevels.Add(level);
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
