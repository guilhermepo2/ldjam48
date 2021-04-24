using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate bool TurnDelegate();
public delegate void AudioClipDelegate(AudioClip _a);

public class TurnBasedManager : MonoBehaviour {
    public static TurnBasedManager s_Instance;

    [Header("Colors")]
    [SerializeField]
    private Color MoveHighlightColor = Color.blue;
    [SerializeField]
    private Color AttackHighlightColor = Color.red;
    private Tile[] m_AllTiles;
    bool bHasClickedToMove = false;


    private List<Actor> m_KnownActors;
    public bool AddActor(Actor _a) {
        m_KnownActors.Add(_a);
        return true;
    }

    private int m_CurrentActorTurn = 0;
    public event System.Action OnTurnWasTaken;
    AudioClipDelegate SoundEffectFunction;

    private void Awake() {
        if (s_Instance == null) {
            s_Instance = this;
        } else {
            Destroy(gameObject);
        }

        m_KnownActors = new List<Actor>();
        m_AllTiles = FindObjectsOfType<Tile>();
        // OnTurnWasTaken += ResetAllTiles;
    }

    private void Update() {
        ProcessCurrentActorTurn();
    }

    void ProcessCurrentActorTurn() {
        if (m_KnownActors.Count == 0 || m_CurrentActorTurn < 0 || m_CurrentActorTurn > m_KnownActors.Count) {
            return;
        }

        if (m_KnownActors[m_CurrentActorTurn].TakeTurn()) {
            if (m_KnownActors.Count > 0) {
                m_CurrentActorTurn = ((m_CurrentActorTurn + 1) % m_KnownActors.Count);
            }

            OnTurnWasTaken?.Invoke();
        }
    }

    // TODO: Some way to get all actors around...
    // Add actor to list
    // remove actor from list

    // =================================================================================================
    //
    //  COMBAT
    //
    // =================================================================================================
    #region HANDLING COMBAT
    public void HandleCombat(DynamicActor _AttackingActor, DynamicActor _BeingAttackedActor) {
        _AttackingActor.DealtDamage();
        _BeingAttackedActor.SufferedDamage(_AttackingActor.ActorStats.BaseDamage);
        // ShowFeedbackText(_BeingAttackedActor.CurrentPosition, _AttackingActor.ActorStats.BaseDamage.ToString(), Color.red);
    }
    #endregion

    // =================================================================================================
    //
    //  QUERIES
    //
    // =================================================================================================

    #region QUERIES
    /// <summary>
    /// Check if there is an actor at a given position
    /// </summary>
    /// <param name="_Position">Position to check</param>
    /// <returns>Whether there is an actor at that position or not</returns>
    public bool IsThereAnActorAt(Vector2 _Position) {
        foreach (Actor _Actor in m_KnownActors) {
            if (_Actor.CurrentPosition == _Position) {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Check what actors is in that position
    /// </summary>
    /// <param name="_Position">Position to check for an actor</param>
    /// <returns>The actor that is on that position</returns>
    public Actor WhatActorIsAt(Vector2 _Position) {
        foreach (Actor _Actor in m_KnownActors) {
            if (_Actor.CurrentPosition == _Position) {
                return _Actor;
            }
        }

        return null;
    }

    /// <summary>
    /// Check whether or not there are enemies alive on the Turn List
    /// </summary>
    /// <returns>If there are enemies alive or not</returns>
    public bool AreThereEnemiesAlive() {
        foreach (Actor _Actor in m_KnownActors) {
            if (_Actor.ActorType == EActorType.EAT_Enemy) {
                return false;
            }
        }

        return true;
    }
    #endregion

    // =================================================================================================
    //
    //  TACTICS
    //
    // =================================================================================================

    public void Move() {
        if (m_KnownActors[m_CurrentActorTurn].ActorType == EActorType.EAT_Player) {
            if (bHasClickedToMove) {
                ResetAllTiles();
            } else {
                HighlightTilesInRange(m_KnownActors[m_CurrentActorTurn].CurrentPosition, MoveHighlightColor, true);
                bHasClickedToMove = true;
            }
        }
    }

    public void HighlightTilesInRange(Vector2 OriginalPosition, Color _color, bool HasToBeClear) {
        foreach (Tile _tile in m_AllTiles) {
            Vector2 TilePosition = _tile.transform.position;
            bool IsClear = true;
            if (HasToBeClear) IsClear = _tile.IsClear();

            if (TilePosition != OriginalPosition && MathUtilities.ManhattanDistance(OriginalPosition, TilePosition) <= 2 && IsClear) {
                _tile.ShowHighlight(_color);
            }
        }
    }

    public void PlayerClickedTile(Tile _tile) {
        Debug.Log("Hello");

        if (m_KnownActors[m_CurrentActorTurn].ActorType == EActorType.EAT_Player && bHasClickedToMove) {
            Vector2 TilePosition = _tile.transform.position;
            Vector2 CharacterPosition = m_KnownActors[m_CurrentActorTurn].CurrentPosition;

            if (MathUtilities.ManhattanDistance(TilePosition, CharacterPosition) <= 2) {
                ResetAllTiles();
                MoveActor(m_KnownActors[m_CurrentActorTurn], TilePosition);
            }

            bHasClickedToMove = false;
        }
    }

    private void MoveActor(Actor _actor, Vector2 _position) {
        StartCoroutine(MoveActorRoutine(_actor, _position));
    }

    private IEnumerator MoveActorRoutine(Actor _actor, Vector2 _position) {
        Vector3 OriginalPosition = _actor.CurrentPosition;

        Vector3 XAxisGoalPosition = new Vector3(_position.x, OriginalPosition.y, OriginalPosition.z);
        while (_actor.transform.position.x != _position.x) {
            _actor.transform.position = Vector3.MoveTowards(_actor.transform.position, XAxisGoalPosition, 5.0f * Time.deltaTime);
            yield return null;
        }

        Vector3 YAxisGoal = new Vector3(_position.x, _position.y, OriginalPosition.z);
        while (_actor.transform.position.y != _position.y) {
            _actor.transform.position = Vector3.MoveTowards(_actor.transform.position, YAxisGoal, 5.0f * Time.deltaTime);
            yield return null;
        }

        _actor.CurrentPosition = _actor.transform.position;
        yield return null;
    }

    private void ResetAllTiles() {
        bHasClickedToMove = false;

        foreach (Tile _tile in m_AllTiles) {
            _tile.Reset();
        }
    }

    #region Helpers
    public void PlaySoundEffect(AudioClip _a) => SoundEffectFunction?.Invoke(_a);
    #endregion
}