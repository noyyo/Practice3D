using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerStateMachine : StateMachine
{
    public Player Player { get; }
    public LayerMask GroundLayerMask;

    // States
    public PlayerIdleState IdleState { get; }
    public PlayerWalkState WalkState { get; }
    public PlayerRunState RunState { get; }
    public PlayerJumpState JumpState { get; }
    public PlayerFallState FallState { get; }
    public Vector2 MovementInput { get; set; }
    public float MovementSpeed { get; private set; }
    public float RotationDamping { get; private set; }
    public float MovementSpeedModifier { get; set; } = 1f;
    public float JumpForce { get; set; }
    public PlayerStateMachine(Player player)
    {
        this.Player = player;

        IdleState = new PlayerIdleState(this);
        WalkState = new PlayerWalkState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);
        FallState = new PlayerFallState(this);

        MovementSpeed = player.Data.GroundedData.BaseSpeed;
        RotationDamping = player.Data.GroundedData.BaseRotationDamping;
        GroundLayerMask = LayerMask.NameToLayer("Everything");
    }
    public void Move(Vector3 direction)
    {
        Player.Rigidbody.velocity = new Vector3(direction.x, Player.Rigidbody.velocity.y, direction.z);

    }
    public bool IsGrounded()
    {

        Transform playerTransform = Player.transform;
        Ray[] rays = new Ray[]
        {
            new Ray(playerTransform.position, Vector3.down),
            new Ray(playerTransform.position + (playerTransform.forward * 0.25f) + (playerTransform.right * 0.25f), Vector3.down),
            new Ray(playerTransform.position + (playerTransform.forward * 0.25f) - (playerTransform.right * 0.25f), Vector3.down),
            new Ray(playerTransform.position - (playerTransform.forward * 0.25f) + (playerTransform.right * 0.25f), Vector3.down),
            new Ray(playerTransform.position - (playerTransform.forward * 0.25f) - (playerTransform.right * 0.25f), Vector3.down),
        };
        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, GroundLayerMask))
            {
                return true;
            }
        }

        return false;
    }
}
