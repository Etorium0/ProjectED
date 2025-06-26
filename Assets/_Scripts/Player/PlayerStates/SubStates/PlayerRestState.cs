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
        
        // Tìm và kích hoạt bonfire
        ActivateNearbyBonfire();
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

    private void ActivateNearbyBonfire()
    {
        // Tìm bonfire gần nhất
        Bonfire[] bonfires = Object.FindObjectsOfType<Bonfire>();
        Bonfire nearestBonfire = null;
        float nearestDistance = float.MaxValue;
        float maxDistance = 3f; // Khoảng cách tối đa
        
        foreach (Bonfire bonfire in bonfires)
        {
            float distance = Vector2.Distance(player.transform.position, bonfire.transform.position);
            if (distance < maxDistance && distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestBonfire = bonfire;
            }
        }
        
        if (nearestBonfire != null)
        {
            // Gọi UseBonfire trực tiếp
            var playerCollider = player.GetComponent<Collider2D>();
            if (playerCollider != null)
            {
                nearestBonfire.SendMessage("UseBonfire", playerCollider, SendMessageOptions.DontRequireReceiver);
                Debug.Log("Bonfire activated via RestState");
            }
        }
    }
}