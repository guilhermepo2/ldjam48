using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonLevelEditor : MonoBehaviour {

    private const string m_prependPath = "/Levels";

    [Header("Feedback Sprites")]
    public Sprite Ground;
    public Sprite Wall;
    public Sprite StartingPosition;
    public Sprite Upstairs;
    public Sprite Downstairs;

    [Header("Level Editor")]
    public GameObject LevelEditorCell;

    [Header("UI")]
    public InputField FilenameInput;

    private int Width = 5;
    private int Height = 5;

    public static DungeonLevelEditor instance;
    private DungeonLevelEditorCell[] m_AllCells;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        for(int i = -Width; i <= Width; i++) {
            for(int j = -Height; j <= Height; j++) {
                DungeonLevelEditorCell dlec = Instantiate(LevelEditorCell, new Vector3(i, j, 0), Quaternion.identity).GetComponent<DungeonLevelEditorCell>();
                dlec.Initialize();
            }
        }

        m_AllCells = FindObjectsOfType<DungeonLevelEditorCell>();
    }

    public void SaveFile() {
        string Filename = FilenameInput.text;

        DungeonObject dungeon = new DungeonObject();

        foreach(DungeonLevelEditorCell cell in m_AllCells) {
            switch(cell.CurrentCellType) {
                case DungeonLevelEditorCell.ECellType.Ground:
                    dungeon.GroundPositions.Add(cell.transform.position);
                    break;
                case DungeonLevelEditorCell.ECellType.Wall:
                    dungeon.WallPositions.Add(cell.transform.position);
                    break;
                case DungeonLevelEditorCell.ECellType.StartPosition:
                    dungeon.StartPosition = cell.transform.position;
                    break;
                case DungeonLevelEditorCell.ECellType.Upstairs:
                    dungeon.UpstairsPosition = cell.transform.position;
                    break;
                case DungeonLevelEditorCell.ECellType.Downstairs:
                    dungeon.DownstairsPosition = cell.transform.position;
                    break;
            }
        }

        DungeonHelpers.SaveDungeon(dungeon, $"{m_prependPath}/{Filename}");
    }
}
