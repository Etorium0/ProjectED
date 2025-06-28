using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Etorium.CoreSystem;

public class BossHealState : State
{
    protected Boss boss;
    protected D_HealState healStateData;
    
    protected float healTimer;
    protected bool hasHealed;
    protected bool isInterrupted;
    
    protected Stats stats;
    protected Movement movement;
    protected DamageReceiver damageReceiver;
    protected PoiseDamageReceiver poiseDamageReceiver;

    public BossHealState(Entity entity, FiniteStateMachine stateMachine, string animBoolName, D_HealState healStateData, Boss boss)
        : base(entity, stateMachine, animBoolName)
    {
        this.boss = boss;
        this.healStateData = healStateData;
    }

    public override void Enter()
    {
        base.Enter();
        
        stats = entity.Core.GetCoreComponent<Stats>();
        movement = entity.Core.GetCoreComponent<Movement>();
        damageReceiver = entity.Core.GetCoreComponent<DamageReceiver>();
        poiseDamageReceiver = entity.Core.GetCoreComponent<PoiseDamageReceiver>();
        
        healTimer = 0f;
        hasHealed = false;
        isInterrupted = false;
        
        // Dừng boss lại khi heal
        if (movement != null)
        {
            movement.SetVelocityX(0f);
            movement.SetVelocityY(0f);
        }
        
        // Disable damage receivers để boss bất bại
        if (damageReceiver != null)
        {
            damageReceiver.enabled = false;
        }
        if (poiseDamageReceiver != null)
        {
            poiseDamageReceiver.enabled = false;
        }
        
        // Boss bất bại trong suốt quá trình heal
        Debug.Log($"Boss bắt đầu heal! Máu hiện tại: {stats.Health.CurrentValue}/{stats.Health.MaxValue} - BOSS BẤT BẠI!");
    }

    public override void Exit()
    {
        base.Exit();
        
        // Enable lại damage receivers
        if (damageReceiver != null)
        {
            damageReceiver.enabled = true;
        }
        if (poiseDamageReceiver != null)
        {
            poiseDamageReceiver.enabled = true;
        }
        
        // Reset trạng thái
        hasHealed = false;
        isInterrupted = false;
        
        // Thông báo cho Boss class biết heal đã hoàn thành
        boss.OnHealComplete();
        
        Debug.Log($"Boss kết thúc heal! Máu cuối: {stats.Health.CurrentValue}/{stats.Health.MaxValue}");
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
        healTimer += Time.deltaTime;
        
        // Đảm bảo boss không di chuyển trong khi heal
        if (movement != null)
        {
            movement.SetVelocityX(0f);
            movement.SetVelocityY(0f);
        }
        
        // Log máu mỗi giây
        if (Mathf.FloorToInt(healTimer) != Mathf.FloorToInt(healTimer - Time.deltaTime))
        {
            Debug.Log($"Boss đang heal... Máu: {stats.Health.CurrentValue}/{stats.Health.MaxValue} (Thời gian: {healTimer:F1}s) - BẤT BẠI!");
        }
        
        // Thực hiện heal sau khi hoàn thành animation
        if (healTimer >= healStateData.healDuration && !hasHealed)
        {
            PerformHeal();
        }
        
        // Kết thúc trạng thái heal ngay sau khi heal xong
        if (hasHealed)
        {
            Debug.Log("Boss đã heal xong, chuyển về walk state!");
            stateMachine.ChangeState(boss.walkState);
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        
        // Đảm bảo boss không di chuyển trong PhysicsUpdate
        if (movement != null)
        {
            movement.SetVelocityX(0f);
            movement.SetVelocityY(0f);
        }
    }

    protected virtual void PerformHeal()
    {
        if (hasHealed) return;
        
        float oldHealth = stats.Health.CurrentValue;
        
        // Tính toán lượng heal (hồi đầy)
        float healAmount = stats.Health.MaxValue * healStateData.healAmount;
        stats.Health.Increase(healAmount);
        
        // Phát hiệu ứng
        if (healStateData.healParticleEffect != null)
        {
            GameObject.Instantiate(healStateData.healParticleEffect, entity.transform.position, Quaternion.identity);
        }
        
        hasHealed = true;
        
        Debug.Log($"Boss đã heal! Máu tăng từ {oldHealth} lên {stats.Health.CurrentValue}/{stats.Health.MaxValue} (+{healAmount:F0}) - BẤT BẠI!");
    }
}
