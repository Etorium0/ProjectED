﻿using System.Collections;
using System.Collections.Generic;
using Etorium.CoreSystem;
using UnityEngine;

public class StunState : State {
	private Movement Movement { get => movement ?? core.GetCoreComponent(ref movement); }
	private CollisionSenses CollisionSenses { get => collisionSenses ?? core.GetCoreComponent(ref collisionSenses); }

	private Movement movement;
	private CollisionSenses collisionSenses;

	protected D_StunState stateData;

	protected bool isStunTimeOver;
	protected bool isGrounded;
	protected bool isMovementStopped;
	protected bool performCloseRangeAction;
	protected bool isPlayerInMinAgroRange;

	public StunState(Entity etity, FiniteStateMachine stateMachine, string animBoolName, D_StunState stateData) : base(etity, stateMachine, animBoolName) {
		this.stateData = stateData;
	}

	public override void DoChecks() {
		base.DoChecks();

		isGrounded = CollisionSenses.Ground;
		performCloseRangeAction = entity.CheckPlayerInCloseRangeAction();
		isPlayerInMinAgroRange = entity.CheckPlayerInMinAgroRange();
	}

	public override void Enter() {
		base.Enter();

		isStunTimeOver = false;
		isMovementStopped = false;
		Movement?.SetVelocity(stateData.stunKnockbackSpeed, stateData.stunKnockbackAngle, entity.lastDamageDirection);

	}

	public override void Exit() {
		base.Exit();
		entity.ResetStunResistance();
	}

	public override void LogicUpdate() {
		base.LogicUpdate();

		if (Time.time >= startTime + stateData.stunTime) {
			isStunTimeOver = true;
		}

		if (isGrounded && Time.time >= startTime + stateData.stunKnockbackTime && !isMovementStopped) {
			isMovementStopped = true;
			Movement?.SetVelocityX(0f);
		}
	}

	public override void PhysicsUpdate() {
		base.PhysicsUpdate();
	}
}
