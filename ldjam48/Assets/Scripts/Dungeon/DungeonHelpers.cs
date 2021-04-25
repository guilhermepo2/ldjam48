using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public static class DungeonHelpers {
    public static DungeonObject LoadDungeon(string Path) {
        string JsonData = File.ReadAllText($"{Application.dataPath}{Path}");
        DungeonObject LoadedDungeon = JsonUtility.FromJson<DungeonObject>(JsonData);
        return LoadedDungeon;
    }

    public static void SaveDungeon(DungeonObject dungeon, string Path) {
        string JsonFile = JsonUtility.ToJson(dungeon, true);
        Debug.Log(JsonFile);
        Debug.Log(Application.dataPath);

        File.WriteAllText($"{Application.dataPath}{Path}", JsonFile);
    }
}
