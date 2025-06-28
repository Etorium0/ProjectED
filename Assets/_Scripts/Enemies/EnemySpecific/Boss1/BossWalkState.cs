using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWalkState : MoveState
{
    private Boss boss;
    private Transform player;
    private Rigidbody2D rb;
    private bool canDoAttack1 = false;
    private bool canDoAttack2 = false;
    private float lastAttackTime;
    private float attackCooldown = 1.5f; // Thêm cooldown để tránh spam attack

    public BossWalkState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_MoveState stateData, Boss boss)
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        rb = boss.GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (!boss.playerFound)
        {
            Debug.Log("Boss lost player - going to idle");
            stateMachine.ChangeState(boss.idleState);
            return;
        }
        
        // Kiểm tra nếu boss cần heal
        if (boss.isHealing)
        {
            Debug.Log("Boss needs to heal - going to heal state");
            stateMachine.ChangeState(boss.healState);
            return;
        }
        
        boss.LookAtPlayer();
        
        if (boss.canDoAttack1InRange || boss.canDoAttack2InRange)
        {
            Debug.Log("Boss detected player in range - going to playerDetected");
            stateMachine.ChangeState(boss.playerDetectedState);
            return;
        }
        
        // Di chuyển về phía player
        Vector2 target = new Vector2(player.position.x, rb.position.y);
        Vector2 newPos = Vector2.MoveTowards(rb.position, target, boss.walkStateData.movementSpeed * Time.fixedDeltaTime);
        rb.MovePosition(newPos);
                
    }
} 