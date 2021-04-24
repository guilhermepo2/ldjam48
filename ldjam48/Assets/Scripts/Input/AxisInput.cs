using UnityEngine;

/// <summary>
/// The Axis Input class uses Unity Axis (Horizontal and Vertical) to detect whether the player is moving or not and to which direction.
/// </summary>
public class AxisInput : BaseInput {
    /// <summary>
    /// How long the player has to hold the input so it is registered every frame while the player is still holding it
    /// </summary>
    private const float km_HoldingTimeToAct = 0.25f;

    /// <summary>
    /// Time that the player is currently holding the input.
    /// </summary>
    private float m_HoldingInputTime;

    public AxisInput() {
        m_MovementDirection = EMovementDirection.EMD_NONE;
        m_HoldingInputTime = 0f;
    }

    public override EMovementDirection TickInput() {
        int horizontalAxis = Mathf.CeilToInt(UnityEngine.Input.GetAxisRaw("Horizontal"));
        int verticalAxis = Mathf.CeilToInt(UnityEngine.Input.GetAxisRaw("Vertical"));

        // Calculating how much time the player has been holding the axis
        // This way the player can move by holding a key instead of having to press it every time
        if (horizontalAxis != 0 || verticalAxis != 0) {
            m_HoldingInputTime += Time.deltaTime;
        }

        // Effectively Processing which movement has to be made
        if (
            // Player pressed something
            (UnityEngine.Input.GetButtonDown("Horizontal") || UnityEngine.Input.GetButtonDown("Vertical"))
            || // or has been holding for enough time
            m_HoldingInputTime > km_HoldingTimeToAct
            ) {
            m_HoldingInputTime = 0f;

            // Horizontal Axis have movement priority over Vertical Axis
            // This cannot be handled in one line because it is needed to ensure player cannot move diagonally
            if (horizontalAxis != 0) {
                m_MovementDirection = InputUtilities.GetMovementDirectionFromVector(new Vector2(horizontalAxis, 0));
            } else {
                m_MovementDirection = InputUtilities.GetMovementDirectionFromVector(new Vector2(0, verticalAxis));
            }

        } else {
            m_MovementDirection = EMovementDirection.EMD_NONE;
        }

        return m_MovementDirection;
    }
}