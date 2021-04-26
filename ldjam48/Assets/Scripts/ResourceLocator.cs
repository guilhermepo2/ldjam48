using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerInventory {
    public int HealthPotions;
    public int Gold;

    public List<int> FoodCount;

    public PlayerInventory() {
        HealthPotions = 0;
        Gold = 0;
        FoodCount = new List<int>();
    }

    public void Reset() {
        HealthPotions = 2;
        Gold = 0;
        for(int i = 0; i < FoodCount.Count; i++) {
            FoodCount[i] = 0;
        }
    }
}

public class PlayerLevelStats {
    public int CurrentLevel;
    public int ExtraStrength;
    public int ExtraConstitution;

    public PlayerLevelStats() {
        CurrentLevel = 1;
        ExtraStrength = 0;
        ExtraConstitution = 0;
    }

    public void Reset() {
        CurrentLevel = 0;
        ExtraConstitution = 0;
        ExtraStrength = 0;
    }
}

public class ResourceLocator : MonoBehaviour {
    public static ResourceLocator instance;

    public bool IsDungeon() { // lol
        return SceneManager.GetActiveScene().name == "Dungeon";
    }

    public bool IsCity() { // LOL!!!
        return SceneManager.GetActiveScene().name == "City";
    }

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
        m_PlayerLevelStats = new PlayerLevelStats();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        Debug.Log("Resource Locator Start");

        if (IsDungeon()) {
            FindObjectOfType<PotionAndGoldContainer>().Apply();
        } else if(IsCity()) { // we are in the city...
            FindObjectOfType<Hero>().InitializeForCity();
        }
    }

    [Header("Monster Database")]
    public GameObject[] MonsterPrefabs;

    [Header("Weapon Database")]
    public Weapon[] Weapons;

    [Header("Food Database")]
    public Food[] Foods;
    public GameObject FoodDropPrefab;

    [Header("Dungeon Settings")]
    public int CurrentDifficulty;
    private List<string> m_AllLevels;
    private List<string> m_AvailableLevels;
    private List<string> m_AlreadyPlayedLevels;
    public GameObject PotionPrefab;
    public GameObject GoldPrefab;

    [Header("Player Equipments")]
    public Weapon PlayerWeapon;
    private PlayerInventory m_PlayerInventory;
    private PlayerLevelStats m_PlayerLevelStats;
    public void IncrementStr() {
        m_PlayerLevelStats.ExtraStrength++;
        m_PlayerLevelStats.CurrentLevel++;
    }

    public void IncrementCon() {
        m_PlayerLevelStats.ExtraConstitution++;
        m_PlayerLevelStats.CurrentLevel++;
    }

    public int CurrentLevel {
        get {
            return m_PlayerLevelStats.CurrentLevel;
        }
    }

    public int PlayerExtraCon {
        get {
            return m_PlayerLevelStats.ExtraConstitution;
        }
    }

    public int PlayerExtraStr {
        get {
            return m_PlayerLevelStats.ExtraStrength;
        }
    }


    public int PotionCount {
        get {
            return m_PlayerInventory.HealthPotions;
        }
    }
    public int GoldCount {
        get {
            return m_PlayerInventory.Gold;
        }
    }

    public void RemoveGold(int _Amount) {
        m_PlayerInventory.Gold = Mathf.Max(m_PlayerInventory.Gold - _Amount, 0);
    }

    public void InitializePlayerInventory() {
        m_PlayerInventory = new PlayerInventory();

        m_PlayerInventory.HealthPotions = 2;

        for (int i = 0; i < Foods.Length; i++) {
            m_PlayerInventory.FoodCount.Add(0);
        }
    }

    private void LoadAllLevels() {
        foreach (string file in System.IO.Directory.GetFiles($"{Application.dataPath}/StreamingAssets/Levels/")) {
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

    public void PlayerPickedUpFood(Food foodObject) {
        for(int i = 0; i < m_PlayerInventory.FoodCount.Count; i++) {
            if(foodObject.FoodName == Foods[i].FoodName) {
                m_PlayerInventory.FoodCount[i]++;
            }
        }

        if(IsDungeon()) {
            foreach(UIFoodContainer container in FindObjectsOfType<UIFoodContainer>()) {
                container.Apply();
            }
        }
    }

    public int GetFoodCountFromFoodObject(Food foodObject) {
        for(int i = 0; i < m_PlayerInventory.FoodCount.Count; i++) {
            if(foodObject.FoodName == Foods[i].FoodName) {
                return m_PlayerInventory.FoodCount[i];
            }
        }

        return 0;
    }

    public void PlayerWantsToEat(Food foodObject) {
        for(int i = 0; i < m_PlayerInventory.FoodCount.Count; i++) {
            if(foodObject.FoodName == Foods[i].FoodName) {
                if(m_PlayerInventory.FoodCount[i] > 0) {
                    FindObjectOfType<Hero>().AddFood(foodObject.CanHealNTimes);
                    m_PlayerInventory.FoodCount[i] -= 1;

                    foreach (UIFoodContainer container in FindObjectsOfType<UIFoodContainer>()) {
                        container.Apply();
                    }
                }
                break;
            }
        }
    }

    public void PlayerGotResource(ResourceDrop.EResource ResourceType, int Amount) {
        switch(ResourceType) {
            case ResourceDrop.EResource.Gold:
                m_PlayerInventory.Gold += Amount;
                break;
            case ResourceDrop.EResource.Potion:
                m_PlayerInventory.HealthPotions += Amount;
                break;
        }

        if(IsDungeon()) {
            FindObjectOfType<PotionAndGoldContainer>().Apply();
        }
    }

    public void PlayerWantsToDrinkPotion() {
        if(m_PlayerInventory.HealthPotions > 0) {
            m_PlayerInventory.HealthPotions -= 1;
            FindObjectOfType<Hero>().GetComponent<DynamicActor>().Heal(15);

            FindObjectOfType<PotionAndGoldContainer>().Apply();
        }
    }

    // TODO
    // Save (lol)
    // Info on the Dungeon Generation
    // Player Info
    // basically everything that has to be persitent between play sessions and scenes should go here!

    // Changing Scenes
    public void GoToDungeonScene() {
        SceneManager.LoadScene("Dungeon");
    }

    public void GoToCityScene() {
        SceneManager.LoadScene("City");
    }

    public void GoToMenu() {
        SceneManager.LoadScene("MainMenu");
    }

    // ------------------------------------------------------------------
    public void GameOver() {
        if(IsDungeon()) {
            DungeonHUD.instance.GameOverPanel.SetActive(true);
            m_PlayerInventory.Reset();
            m_PlayerLevelStats.Reset();
        }

        // there's no way the game will end outside of the dungeon... right?
    }
}
