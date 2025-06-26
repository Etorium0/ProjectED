using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayerDetectedState : PlayerDetectedState
{
    private Boss boss;

    public BossPlayerDetectedState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_PlayerDetected stateData, Boss boss) : base(etity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        if (performCloseRangeAction)
        {
            Debug.Log("Boss executing close range action - attacking");
            // Attack khi player trong close range
            if (boss.canDoAttack1InHitzone && boss.canDoAttack2InHitzone)
            {
                if (Random.Range(0, 2) == 0)
                    stateMachine.ChangeState(boss.attack1State);
                else
                    stateMachine.ChangeState(boss.attack2State);
            }
            else if (boss.canDoAttack1InHitzone)
            {
                stateMachine.ChangeState(boss.attack1State);
            }
            else if (boss.canDoAttack2InHitzone)
            {
                stateMachine.ChangeState(boss.attack2State);
            }
        }
        else if (!isPlayerInMaxAgroRange)
        {

            stateMachine.ChangeState(boss.walkState);
        }
        else if (performLongRangeAction)
        {
            stateMachine.ChangeState(boss.walkState);
        }
        else
        {
            Debug.Log("Boss waiting in PlayerDetected state");
        }
    }
} 