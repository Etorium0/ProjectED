using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack2State : MeleeAttackState
{
    private Boss boss;
    public BossAttack2State(Entity entity, FiniteStateMachine stateMachine, string animBoolName, Transform attackPosition, D_MeleeAttack stateData, Boss boss)
        : base(entity, stateMachine, animBoolName, attackPosition, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();
        // Thực hiện logic tấn công 2
        boss.PerformAttack2();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    
        // Kiểm tra animation đã kết thúc chưa
        if (isAnimationFinished)
        {
            // Quay về playerDetectedState để xử lý logic tiếp theo
            stateMachine.ChangeState(boss.playerDetectedState);
        }
    }
} 