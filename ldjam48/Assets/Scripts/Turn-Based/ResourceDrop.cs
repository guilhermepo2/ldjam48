using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ResourceDrop : MonoBehaviour {
    public enum EResource {
        Potion,
        Gold
    }

    public EResource ResourceType;
    public int Amount;
}
