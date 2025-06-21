using UnityEngine;

public class PlayerRestState : PlayerState
{
    public PlayerRestState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        : base(player, stateMachine, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        player.RB.linearVelocity = Vector2.zero; 
        player.InputHandler.UseRestInput(); 
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (player.InputHandler.RestInput)
        {
            stateMachine.ChangeState(player.IdleState); // hoặc GroundedState
            player.InputHandler.UseRestInput(); // Reset input ngay sau khi dùng!
        }
    }
}