using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSpritEvery : MonoBehaviour {
    public float TimeToFlipSprite;
    private SpriteRenderer m_Sprite;

    private void Awake() {
        m_Sprite = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        StartCoroutine(WaitAndFlipSprite(TimeToFlipSprite));
    }

    private IEnumerator WaitAndFlipSprite(float TimeToWait) {
        yield return new WaitForSeconds(TimeToWait);
        m_Sprite.flipX = !m_Sprite.flipX;
        StartCoroutine(WaitAndFlipSprite(TimeToWait));
    }
}
