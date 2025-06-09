using System;
using UnityEngine;

public class PlayerMoveState : PlayerState
{
    public PlayerMoveState(Player player, PlayerStateMachine stateMachine, string stateName) : base(player, stateMachine, stateName)
    {

    }

    public override void Update()
    {
        base.Update();

        if (player.moveInput.x == 0 && player.moveInput.y == 0)
        {
            stateMachine.ChangeState(player.idleState);
            return;
        }

        float currentSpeed = GetSpeedForForm(player.CurrentForm);
        player.SetVelocity(player.moveInput.x * currentSpeed, player.moveInput.y * currentSpeed);
    }

    private float GetSpeedForForm(FormType currentForm)
    {
        float currentSpeed;

        if (currentForm == FormType.Stone)
            currentSpeed = 20f;
        else if (currentForm == FormType.Veil)
            currentSpeed = 8f;
        else if (currentForm == FormType.Blade)
            currentSpeed = 12f;
        else
            currentSpeed = player.moveSpeed;

        return currentSpeed;
    }
}
