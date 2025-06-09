using UnityEngine;

public class PlayerFormStateMachine
{
    public PlayerState currentState { get; private set; }

    public void InitializeForm(PlayerState _startState)
    {
        currentState = _startState;
        currentState.Enter();
    }

    public void ChangeFormState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    public void UpdateActiveFormState()
    {
        currentState.Update();
    }
}
