using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DungeonLevelEditor : MonoBehaviour {

    private const string m_prependPath = "/Levels";

    public float CameraMoveSpeed = 5.0f;

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

    private int Width = 6;
    private int Height = 6;

    public static DungeonLevelEditor instance;
    private DungeonLevelEditorCell[] m_AllCells;

    private Camera MainCameraReference;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        MainCameraReference = Camera.main;

        for(int i = -Width; i <= Width; i++) {
            for(int j = -Height; j <= Height; j++) {
                DungeonLevelEditorCell dlec = Instantiate(LevelEditorCell, new Vector3(i, j, 0), Quaternion.identity).GetComponent<DungeonLevelEditorCell>();
                dlec.Initialize();
            }
        }

        m_AllCells = FindObjectsOfType<DungeonLevelEditorCell>();
    }

    private void Update() {
        if(Input.GetKey(KeyCode.S)) {
            MainCameraReference.transform.Translate(Vector3.down * Time.deltaTime * CameraMoveSpeed);
        } else if(Input.GetKey(KeyCode.W)) {
            MainCameraReference.transform.Translate(Vector3.up * Time.deltaTime * CameraMoveSpeed);
        } else if(Input.GetKey(KeyCode.D)) {
            MainCameraReference.transform.Translate(Vector3.right * Time.deltaTime * CameraMoveSpeed);
        } else if(Input.GetKey(KeyCode.A)) {
            MainCameraReference.transform.Translate(Vector3.left * Time.deltaTime * CameraMoveSpeed);
        }

        if(Input.mouseScrollDelta.y != 0) {
            MainCameraReference.orthographicSize = MainCameraReference.orthographicSize - Input.mouseScrollDelta.y;
        }
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
