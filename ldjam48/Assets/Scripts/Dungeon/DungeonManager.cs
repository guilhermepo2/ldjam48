using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class DungeonManager : MonoBehaviour {
    [Header("Prefabs")]
    public GameObject DungeonGround;
    public GameObject DungeonWall;
    // public GameObject Upstairs;
    // public GameObject Downstairs;

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
        LoadDungeon("test0.json");
    }

    public void LoadDungeon(string _filepath) {
        DungeonObject LoadedDungeon = DungeonHelpers.LoadDungeon($"{m_prependPath}/{_filepath}");
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
    }

    
}
