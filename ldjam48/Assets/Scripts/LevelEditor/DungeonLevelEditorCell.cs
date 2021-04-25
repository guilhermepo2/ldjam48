using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(SpriteRenderer))]
public class DungeonLevelEditorCell : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public enum ECellType {
        Ground = 0,
        Wall = 1,
        StartPosition = 2,
        Upstairs = 3,
        Downstairs = 4
    }

    [HideInInspector]
    public ECellType CurrentCellType;
    private SpriteRenderer m_SpriteRenderer;

    public void Initialize() {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
        CurrentCellType = ECellType.Ground;
        UpdateSprite();
    }

    #region HOVER HANDLING
    public void OnPointerEnter(PointerEventData pointerEventData) {
        // TODO...
    }

    public void OnPointerExit(PointerEventData pointerEventData) {
        // TODO...
    }
    #endregion

    public void OnPointerClick(PointerEventData eventData) {

        if(eventData.button != PointerEventData.InputButton.Left && eventData.button != PointerEventData.InputButton.Right) {
            return;
        }

        int CellType = (int)CurrentCellType;

        if(eventData.button == PointerEventData.InputButton.Left) {
            CellType += 1;
        } else if(eventData.button == PointerEventData.InputButton.Right) {
            CellType -= 1;
        }

        if(CellType < 0) {
            CellType = 4;
        } else if(CellType > 4) {
            CellType = 0;
        }

        CurrentCellType = (ECellType)CellType;
        UpdateSprite();
    }

    void UpdateSprite() {
        switch(CurrentCellType) {
            case ECellType.Ground:
                m_SpriteRenderer.sprite = DungeonLevelEditor.instance.Ground;
                m_SpriteRenderer.color = new Color(122.0f, 68.0f, 74.0f, 1.0f);
                break;
            case ECellType.Wall:
                m_SpriteRenderer.sprite = DungeonLevelEditor.instance.Wall;
                m_SpriteRenderer.color = Color.white;
                break;
            case ECellType.StartPosition:
                m_SpriteRenderer.sprite = DungeonLevelEditor.instance.StartingPosition;
                m_SpriteRenderer.color = Color.white;
                break;
            case ECellType.Upstairs:
                m_SpriteRenderer.sprite = DungeonLevelEditor.instance.Upstairs;
                m_SpriteRenderer.color = Color.white;
                break;
            case ECellType.Downstairs:
                m_SpriteRenderer.sprite = DungeonLevelEditor.instance.Downstairs;
                m_SpriteRenderer.color = Color.white;
                break;
        }
    }
}
