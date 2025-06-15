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

        AssignSpeed();

        ChangeToAbilityState();
    }

    private void ChangeToAbilityState()
    {
        if (player.currentAbility.ShouldActivate())
        {
            //if(player.CurrentForm == FormType.Stone)
            //{
            //    stateMachine.ChangeState(player.sheildState);
            //}

            //if (player.CurrentForm == FormType.Veil)
            //{
            //    stateMachine.ChangeState(player.invisState);
            //}

            if (player.CurrentForm == FormType.Blade)
            {
                stateMachine.ChangeState(player.dashState);
            }
        }
    }

    private void AssignSpeed()
    {
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
            currentSpeed = 6f;
        else if (currentForm == FormType.Veil)
            currentSpeed = 10f;
        else if (currentForm == FormType.Blade)
            currentSpeed = 15f;
        else
            currentSpeed = player.moveSpeed;

        return currentSpeed;
    }
}
