using System.Collections;
using System.Collections.Generic;
using Etorium.Weapons;
using UnityEngine;

public class PlayerAttackState : PlayerAbilityState
{
    private Weapon weapon;
    private WeaponGenerator weaponGenerator;

    private int inputIndex;

    public PlayerAttackState(
        Player player,
        PlayerStateMachine stateMachine,
        PlayerData playerData,
        string animBoolName,
        Weapon weapon,
        CombatInputs input
    ) : base(player, stateMachine, playerData, animBoolName)
    {
        this.weapon = weapon;
        
        weaponGenerator = weapon.GetComponent<WeaponGenerator>();

        inputIndex = (int)input;

        weapon.OnExit += HandleExit;
        weapon.OnUseInput += HandleUseInput;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        weapon.CurrentInput = player.InputHandler.AttackInputs[inputIndex];
    }
    
    private void HandleWeaponGenerating()
    {
        stateMachine.ChangeState(player.IdleState);
    }

    public override void Enter()
    {
        base.Enter();
        
        weaponGenerator.OnWeaponGenerating += HandleWeaponGenerating;

        weapon.Enter();
    }

    public override void Exit()
    {
        base.Exit();
        
        weaponGenerator.OnWeaponGenerating -= HandleWeaponGenerating;

        weapon.Exit();
    }

    public bool CanTransitionToAttackState() => weapon.CanEnterAttack;

    private void HandleUseInput()
    {
        player.InputHandler.UseAttackInput(inputIndex);
    }

    private void HandleExit()
    {
        AnimationFinishTrigger();
        isAbilityDone = true;
    }
}