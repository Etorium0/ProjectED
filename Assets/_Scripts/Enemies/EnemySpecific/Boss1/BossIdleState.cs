using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdleState : IdleState
{
    private Boss boss;
    public BossIdleState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_IdleState stateData, Boss boss)
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (boss.playerFound)
        {
            stateMachine.ChangeState(boss.walkState);
        }
    }
} 