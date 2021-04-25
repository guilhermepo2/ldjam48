using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonTile : Tile {
    [Header("Colors for Sprite")]
    public Color totallyVisibleColor;
    public Color partiallyVisibleColor;
    public Color nonVisibleColor;

    [Header("Sprite Masks")]
    public string visibleSpriteMask = "Background";
    public string partiallyVisibleSpriteMask = "Partially-Visible-Tile";
    public string nonVisibleSpriteMask = "Not-Visible-Tile";

    private bool m_isTileVisible;
    public bool IsVisible {
        get {
            return m_isTileVisible;
        }
        set {
            m_isTileVisible = value;
        }
    }

    private bool m_wasTileDiscovered;
    public bool WasTileDiscovered {
        get {
            return m_wasTileDiscovered;
        }
        set {
            m_wasTileDiscovered = value;
        }
    }

    private SpriteRenderer m_spriteRenderer;

    [Header("Visibility Settings")]
    public bool IsWall;
    public bool BlockVision;

    public void InitializeTile() {
        m_spriteRenderer = HighlightSpriteRenderer.GetComponent<SpriteRenderer>();
        m_spriteRenderer.enabled = true;
        m_isTileVisible = false;
        m_wasTileDiscovered = false;
        UpdateSpriteRenderer(nonVisibleColor, nonVisibleSpriteMask);
    }

    public void UpdateTile() {
        if (m_isTileVisible) {
            UpdateSpriteRenderer(totallyVisibleColor, visibleSpriteMask);
        } else if (m_wasTileDiscovered) {
            UpdateSpriteRenderer(partiallyVisibleColor, partiallyVisibleSpriteMask);
        } else {
            UpdateSpriteRenderer(nonVisibleColor, nonVisibleSpriteMask);
        }
    }

    private void UpdateSpriteRenderer(Color _color, string _sortingLayer) {
        ShowHighlight(_color);
        // we also have to update the parent's sprite renderer otherwise we can still see stuff when the tiles are partially visible lol
        m_spriteRenderer.transform.parent.GetComponent<SpriteRenderer>().sortingLayerName = _sortingLayer; // nice lmao
        m_spriteRenderer.sortingLayerName = _sortingLayer;
    }
}
