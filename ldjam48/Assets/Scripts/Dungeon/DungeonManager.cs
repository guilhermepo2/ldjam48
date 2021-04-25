using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DungeonManager : MonoBehaviour {
    [Header("Prefabs")]
    public GameObject DungeonGround;
    public GameObject DungeonWall;
    public GameObject Upstairs;
    public GameObject Downstairs;

    // ---------------------------------------
    // Monster Spawning
    private int m_NextSpawn = 5;
    private int m_MinToNextSpawn = 20;
    private int m_MaxToNextSpawn = 30;
    private int m_TurnCounter;

    public static DungeonManager instance;

    private const string m_prependPath = "/Levels";
    private List<DungeonTile> m_AllTiles;
    public bool HasTiles {
        get {
            return m_AllTiles.Count != 0;
        }
    }

    public DungeonTile GetTileOnPosition(float x, float y) {
        foreach(DungeonTile dt in m_AllTiles) {
            if(dt.transform.position == new Vector3(x, y, dt.transform.position.z)) {
                return dt;
            }
        }

        return null;
    }

    private void Awake() {
        if(instance == null) {
            instance = this;
        }

        m_AllTiles = new List<DungeonTile>();
        
    }

    private void Start() {
        string Level = ResourceLocator.instance.GetPlayableLevel();
        Debug.Log($"Loading Dungeon {Level}");
        LoadDungeon(Level);
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorMoved += ProcessTurn;
    }

    public void GoDeeper() {
        // clean up current floor
        foreach(DungeonTile dt in m_AllTiles) {
            Destroy(dt.gameObject);
        }
        m_AllTiles.Clear();

        // clean up actors (except player)
        foreach(Actor _a in FindObjectsOfType<Actor>()) {
            if(_a.ActorType != EActorType.EAT_Player) {
                _a.ForceDestroy();
            }
        }

        ResourceLocator.instance.GoDeeper();
        string Level = ResourceLocator.instance.GetPlayableLevel();
        LoadDungeon(Level);
    }

    private void ProcessTurn() {
        m_TurnCounter++;

        if(m_TurnCounter == m_NextSpawn) {
            SpawnRandomMonster();
            m_NextSpawn += Random.Range(m_MinToNextSpawn, m_MaxToNextSpawn);
        }
    }

    public void LoadDungeon(string _filepath) {
        DungeonObject LoadedDungeon = DungeonHelpers.LoadDungeon(_filepath);
        InstantiateDungeon(LoadedDungeon);
    }

    private void InstantiateDungeon(DungeonObject TheDungeon) {
        foreach(Vector2 WallPosition in TheDungeon.WallPositions) {
            DungeonTile dt = Instantiate(DungeonWall, WallPosition, Quaternion.identity).GetComponent<DungeonTile>();
            m_AllTiles.Add(dt);
        }

        foreach(Vector2 GroundPosition in TheDungeon.GroundPositions) {
            DungeonTile dt = Instantiate(DungeonGround, GroundPosition, Quaternion.identity).GetComponent<DungeonTile>();
            m_AllTiles.Add(dt);
        }

        DungeonTile up = Instantiate(Upstairs, TheDungeon.UpstairsPosition, Quaternion.identity).GetComponent<DungeonTile>();
        m_AllTiles.Add(up);

        DungeonTile down = Instantiate(Downstairs, TheDungeon.DownstairsPosition, Quaternion.identity).GetComponent<DungeonTile>();
        m_AllTiles.Add(down);

        DungeonTile extra = Instantiate(DungeonGround, TheDungeon.StartPosition, Quaternion.identity).GetComponent<DungeonTile>();
        m_AllTiles.Add(extra);

        Hero theHero = FindObjectOfType<Hero>();
        theHero.InitializeHero();
        theHero.UpdatePosition(TheDungeon.StartPosition);

        foreach(DungeonTile dt in m_AllTiles) {
            Debug.Log("Initializing all Tiles...");
            dt.InitializeTile();
        }

        FindObjectOfType<FourthDimension.Roguelike.FieldOfView>().InitializeFieldOfView(TheDungeon.StartPosition);
    }

    #region Monster Spawning
    private void SpawnRandomMonster() {
        
        Debug.Log("Spawning Monster");

        GameObject MonsterToSpawn = ResourceLocator.instance.MonsterPrefabs.RandomOrDefault();
        List<Vector3> PossiblePositions = new List<Vector3>();

        foreach(DungeonTile dt in m_AllTiles) {
            if(!dt.IsVisible && !dt.IsWall) {
                PossiblePositions.Add(dt.transform.position);
            }
        }

        if(PossiblePositions.Count > 0) {
            Instantiate(MonsterToSpawn, PossiblePositions.RandomOrDefault(), Quaternion.identity);
        }
    }
    #endregion

}
