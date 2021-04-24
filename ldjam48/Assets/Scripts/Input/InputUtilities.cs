using UnityEngine;

public static class InputUtilities {
    /// <summary>
    /// Converts a Vector2 to a Movement Direction
    /// </summary>
    /// <param name="_input">Vector2 direction</param>
    /// <returns>Movement Direction</returns>
    public static EMovementDirection GetMovementDirectionFromVector(Vector2 _input) {
        if (_input.x != 0) {
            if (_input.x > 0) {
                return EMovementDirection.EMD_RIGHT;
            } else {
                return EMovementDirection.EMD_LEFT;
            }
        } else if (_input.y != 0) {
            if (_input.y > 0) {
                return EMovementDirection.EMD_UP;
            } else {
                return EMovementDirection.EMD_DOWN;
            }
        }

        return EMovementDirection.EMD_NONE;
    }

    /// <summary>
    /// Converts a Movement Direction to a Vector2 direction
    /// </summary>
    /// <param name="_direction">Movement Direction</param>
    /// <returns>Vector2 direction</returns>
    public static Vector2 GetMovementVectorFromDirection(EMovementDirection _direction) {
        switch (_direction) {
            case EMovementDirection.EMD_UP:
                return Vector2.up;
            case EMovementDirection.EMD_RIGHT:
                return Vector2.right;
            case EMovementDirection.EMD_DOWN:
                return Vector2.down;
            case EMovementDirection.EMD_LEFT:
                return Vector2.left;
        }

        return Vector2.zero;
    }
}