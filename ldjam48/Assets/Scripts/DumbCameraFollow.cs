using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumbCameraFollow : MonoBehaviour {
    private DynamicActor m_HeroRef;

    private void Start() {
        m_HeroRef = FindObjectOfType<Hero>().GetComponent<DynamicActor>();
    }

    private void Update() {
        if(m_HeroRef != null) {
            transform.position = new Vector3(m_HeroRef.CurrentPosition.x, m_HeroRef.CurrentPosition.y, transform.position.z);
        }
    }
}
