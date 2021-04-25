using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorHealthComponent : MonoBehaviour {
    private int m_MaxHealth;
    public int MaxHealth {
        get {
            return m_MaxHealth;
        }
    }

    private int m_CurrentHealth;
    public int CurrentHealth {
        get {
            return m_CurrentHealth;
        }
    }


    public bool IsDead {
        get {
            return CurrentHealth <= 0;
        }
    }

    public void SetMaxHealth(int _MaxHealth) {
        m_MaxHealth = _MaxHealth;
        m_CurrentHealth = m_MaxHealth;
    }

    public void Heal(int Amount) {
        m_CurrentHealth = Mathf.Min(CurrentHealth + Amount, MaxHealth);
    }

    public void TakeDamage(int Amount) {
        m_CurrentHealth = Mathf.Max(CurrentHealth - Amount, 0);
    }
}
