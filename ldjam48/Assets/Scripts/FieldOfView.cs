using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FourthDimension.Roguelike {

    #region Bob Nystrom Shadow Cast Helper Classes
    public class ShadowLine {
        List<Shadow> m_shadows;

        public bool IsFullShadow {
            get {
                return m_shadows.Count == 1 && m_shadows[0].start == 0 && m_shadows[0].end == 1;
            }
        }

        public ShadowLine() {
            m_shadows = new List<Shadow>();
        }

        /// <summary>
        /// Add a new shadow to the Shadow Line.
        /// </summary>
        /// <param name="_shadowToAdd">Shadow to add.</param>
        public void AddShadowToLine(Shadow _shadowToAdd) {
            // First we have to figure out where to slot the new shadow in the list.
            int index = 0;
            for (; index < m_shadows.Count; index++) {
                // We stop when hit an intersetion point.
                if (m_shadows[index].start >= _shadowToAdd.start) {
                    break;
                }
            }

            // We know where the shadow is going to be, now we check if it overlaps the previous or next.
            Shadow previousOverlappedShadow = null;
            if (index > 0 && m_shadows[index - 1].end > _shadowToAdd.start) {
                previousOverlappedShadow = m_shadows[index - 1];
            }

            Shadow nextOverlappedShadow = null;
            if (index < m_shadows.Count && m_shadows[index].start < _shadowToAdd.end) {
                nextOverlappedShadow = m_shadows[index];
            }

            // Inserting the shadow and unifying with overlapping shadows.
            if (nextOverlappedShadow != null) {
                if (previousOverlappedShadow != null) {
                    // overlaps both, so unify one and delete the other
                    previousOverlappedShadow.end = nextOverlappedShadow.end;
                    m_shadows.RemoveAt(index);
                } else {
                    // overlaps the next one, so unify it with that.
                    nextOverlappedShadow.start = _shadowToAdd.start;
                }
            } else {
                if (previousOverlappedShadow != null) {
                    // Overlaps the previous one, so unify it with that
                    previousOverlappedShadow.end = _shadowToAdd.end;
                } else {
                    // Does not overlap anything, so insert.
                    m_shadows.Insert(index, _shadowToAdd);
                }
            }
        }

        /// <summary>
        /// Verifying if any shadow in the line covers a tile.
        /// </summary>
        /// <param name="_projection">Shadow tile to be verified.</param>
        /// <returns>True if the tile is covered by this shadow, false otherwise.</returns>
        public bool IsInShadow(Shadow _projection) {
            foreach (Shadow shadow in m_shadows) {
                if (shadow.ContainsOther(_projection)) {
                    return true;
                }
            }

            return false;
        }
    }

    public class Shadow {
        public float start;
        public float end;

        public Shadow(float _start, float _end) {
            start = _start;
            end = _end;
        }

        /// <summary>
        /// Check if a shadow is completely covered by this shadow
        /// </summary>
        /// <param name="_other">Shadow tile to be checked</param>
        /// <returns>True if _other if completely covered by this shadow, false otherwise.</returns>
        public bool ContainsOther(Shadow _other) {
            return (start <= _other.start && end >= _other.end);
        }
    }
    #endregion

    public class FieldOfView : MonoBehaviour {
        public enum EFieldOfViewMethod {
            BobNystromShadowCast
        }

        private DungeonTile[] m_AllTiles;

        private DungeonTile GetTileOnPosition(float x, float y) {
            for (int i = 0; i < m_AllTiles.Length; i++) {
                if (m_AllTiles[i].transform.position == new Vector3(x, y, m_AllTiles[i].transform.position.z)) {
                    return m_AllTiles[i];
                }
            }

            return null;
        }

        private void Start() {
            m_AllTiles = FindObjectsOfType<DungeonTile>();
        }

        private const EFieldOfViewMethod km_method = EFieldOfViewMethod.BobNystromShadowCast;
        private const bool km_fovLightWalls = true;
        private const int km_fovTorchRadius = 5;
        private const int km_maxDistance = 15;

        public void InitializeFieldOfView(Vector2 _position) {
            RefreshVisibility(_position);
        }

        public void RefreshVisibility(Vector2 _originPosition) {
            
            if(m_AllTiles.Length == 0) {
                // we don't refresh if there is no tiles... (i.e. we are in the city, or in case something went very wrong)
                return;
            }

            DungeonTile TileInThisPosition = GetTileOnPosition(_originPosition.x, _originPosition.y);
            TileInThisPosition.WasTileDiscovered = true;
            TileInThisPosition.IsVisible = true;
            TileInThisPosition.UpdateTile();

            switch (km_method) {
                case EFieldOfViewMethod.BobNystromShadowCast:
                    for (int octant = 0; octant < 8; octant++) {
                        RefreshOctant2(_originPosition, octant);
                    }
                    break;
            }
        }

        #region Bob Nystrom Shadow Cast
        private void RefreshOctant2(Vector2 _originPosition, int _octant) {
            ShadowLine line = new ShadowLine();
            bool fullShadow = false;

            for (int row = 1; row < km_maxDistance; row++) {
                for (int col = 0; col <= row; col++) {
                    Vector2 position = _originPosition + ConvertPositionToOctantPosition(row, col, _octant);
                    DungeonTile CurrentTile = GetTileOnPosition(position.x, position.y);

                    if (CurrentTile == null) {
                        continue;
                    }

                    if (fullShadow) {
                        CurrentTile.IsVisible = false;
                    } else {

                        Shadow projection = ProjectTile(row, col);

                        // Setting Visibility of this tile...
                        bool visible = !line.IsInShadow(projection);
                        CurrentTile.IsVisible = visible;


                        if (visible) {
                            CurrentTile.WasTileDiscovered = true;
                        }


                        if (visible && (CurrentTile.IsWall || CurrentTile.BlockVision)) {
                            line.AddShadowToLine(projection);
                            fullShadow = line.IsFullShadow;
                        }

                    }

                    CurrentTile.UpdateTile();
                }
            }
        }

        private Vector2 ConvertPositionToOctantPosition(int _row, int _col, int _octant) {
            switch (_octant) {
                case 0:
                    return new Vector2(_col, _row);
                case 1:
                    return new Vector2(_row, _col);
                case 2:
                    return new Vector2(_row, -_col);
                case 3:
                    return new Vector2(_col, -_row);
                case 4:
                    return new Vector2(-_col, -_row);
                case 5:
                    return new Vector2(-_row, -_col);
                case 6:
                    return new Vector2(-_row, _col);
                case 7:
                    return new Vector2(-_col, _row);
            }

            return Vector2.zero;
        }

        /// <summary>
        /// Returns the SLOPE corresponding to the current (row, col) position
        /// </summary>
        /// <param name="_row">Current row</param>
        /// <param name="_col">Current col</param>
        /// <returns>Returns a slope indicating the Projection of a tile.</returns>
        private Shadow ProjectTile(int _row, int _col) {
            // forcing row and col to be float so we don't get bamboozled by int/float conversion somewhere
            float row = _row;
            float col = _col;

            float topLeft = (col / (row + 2));
            float bottomRight = (col + 1) / (row + 1);

            return new Shadow(topLeft, bottomRight);
        }
        #endregion

    }
}