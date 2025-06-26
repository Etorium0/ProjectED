using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack1State : MeleeAttackState
{
    private Boss boss;
    public BossAttack1State(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Boss boss)
        : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        // Thực hiện logic tấn công 1
        boss.PerformAttack1();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        Debug.Log($"Boss Attack1 - isAnimationFinished: {isAnimationFinished}");
        
        if (isAnimationFinished)
        {
            Debug.Log("Animation finished, changing back to player detected state...");
            // Quay về playerDetectedState để xử lý logic tiếp theo
            stateMachine.ChangeState(boss.playerDetectedState);
        }
    }
} 