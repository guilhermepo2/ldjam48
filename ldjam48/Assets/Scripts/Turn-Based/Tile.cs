using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    [SerializeField]
    private SpriteRenderer HighlightSpriteRenderer;

    [Header("Tile Configurations")]
    [SerializeField]
    private LayerMask ObstacleLayers;

    private void Start() {
        Reset();
    }

    public bool IsClear() {
        Collider2D Obstacle = Physics2D.OverlapCircle(transform.position, 0.15f, ObstacleLayers);
        return Obstacle == null;
    }

    public void ShowHighlight(Color _color) {
        HighlightSpriteRenderer.color = _color;
        HighlightSpriteRenderer.enabled = true;
    }

    public void Reset() {
        HighlightSpriteRenderer.enabled = false;
    }

    public void OnMouseDown() {
        TurnBasedManager.s_Instance.PlayerClickedTile(this);
    }
}