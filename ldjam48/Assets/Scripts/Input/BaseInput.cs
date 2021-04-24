/*
 * MIT License
 * Copyright (c) 2019
 * Code written by Guilherme de Oliveira
 */

public enum EMovementDirection {
    EMD_NONE = 0,
    EMD_UP = 10,
    EMD_RIGHT = 20,
    EMD_DOWN = 30,
    EMD_LEFT = 40,
    EMD_MAX = 999
}

public abstract class BaseInput {
    /// <summary>
    /// Direction that the player is currently movement towards
    /// This value is stored so it can be accessed in the following frames, a.k.a. Input Buffering
    /// </summary>
    protected EMovementDirection m_MovementDirection;

    /// <summary>
    /// Do all the necessary procedures to see if the player is moving or not.
    /// </summary>
    /// <returns>The direction the player pressed to move</returns>
    public abstract EMovementDirection TickInput();
}