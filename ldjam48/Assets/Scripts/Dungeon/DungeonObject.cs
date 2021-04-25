using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class DungeonObject {
    public List<Vector2> GroundPositions;
    public List<Vector2> WallPositions;
    public Vector2 StartPosition;
    public Vector2 UpstairsPosition;
    public Vector2 DownstairsPosition;
    public int Difficulty;

    public DungeonObject() {
        GroundPositions = new List<Vector2>();
        WallPositions = new List<Vector2>();
        StartPosition = Vector2.zero;
        UpstairsPosition = Vector2.zero;
        DownstairsPosition = Vector2.zero;
        Difficulty = 0;
    }
}
