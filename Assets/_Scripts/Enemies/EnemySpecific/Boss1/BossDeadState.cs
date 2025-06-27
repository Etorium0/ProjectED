using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDeadState : DeadState
{
    private Boss boss;
    public BossDeadState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_DeadState stateData, Boss boss)
        : base(entity, stateMachine, animBoolName, stateData)
    {
        this.boss = boss;
    }

    public override void Enter()
    {
        base.Enter();

        boss.anim.SetBool("Dead", true);
        
        boss.bossDied.Invoke();
    }
} 