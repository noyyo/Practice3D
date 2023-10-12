using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : PlayerAirState
{
    public PlayerJumpState(PlayerStateMachine playerStateMachine) : base(playerStateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.JumpForce = stateMachine.Player.Data.AirData.JumpForce;
        stateMachine.Player.Rigidbody.AddForce(new Vector3(0, stateMachine.JumpForce,0), ForceMode.Impulse);
        base.Enter();
        StartAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.Player.AnimationData.JumpParameterHash);
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (stateMachine.Player.Rigidbody.velocity.y < 0)
        {
            stateMachine.ChangeState(stateMachine.FallState);
            return;
        }
    }
}
