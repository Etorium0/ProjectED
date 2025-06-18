using UnityEngine;

public class PlayerRestState : PlayerState
{
    public PlayerRestState(Player player, PlayerStateMachine stateMachine, PlayerData playerData, string animBoolName)
        : base(player, stateMachine, playerData, animBoolName) { }

    public override void Enter()
    {
        base.Enter();
        player.Anim.SetBool("rest", true);
        player.RB.linearVelocity = Vector2.zero; // Sửa lại thành velocity thay vì linearVelocity (Unity dùng velocity)
        player.InputHandler.UseRestInput(); // Reset luôn khi vào
    }

    public override void Exit()
    {
        base.Exit();
        player.Anim.SetBool("rest", false);
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