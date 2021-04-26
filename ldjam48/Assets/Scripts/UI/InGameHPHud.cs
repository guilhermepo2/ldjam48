using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameHPHud : MonoBehaviour {

    private Text m_HpText;

    private ActorHealthComponent m_HeroHealth;

    private void Awake() {
        m_HpText = GetComponent<Text>();
        m_HeroHealth = FindObjectOfType<Hero>().GetComponent<ActorHealthComponent>();
    }

    private void Start() {
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorWasHit += UpdateHealth;
        FindObjectOfType<Hero>().GetComponent<DynamicActor>().OnActorHealed += UpdateHealth;
        FindObjectOfType<Hero>().OnHeroInitialized += UpdateHealth;
    }

    private void UpdateHealth() {
        m_HpText.text = $"HP {m_HeroHealth.CurrentHealth}/{m_HeroHealth.MaxHealth}";
    }
}
