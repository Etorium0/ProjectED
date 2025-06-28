using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Etorium.CoreSystem;

public class Boss : Entity
{
    public BossIdleState idleState { get; private set; }
    public BossWalkState walkState { get; private set; }
    public BossAttack1State attack1State { get; private set; }
    public BossAttack2State attack2State { get; private set; }
    public BossDeadState deadState { get; private set; }
    public BossPlayerDetectedState playerDetectedState { get; private set; }
    public BossHealState healState { get; private set; }
    // Thêm các state khác nếu cần

    [Header("Boss Data")]
    public D_IdleState idleStateData;
    public D_MoveState walkStateData;
    public D_MeleeAttack attack1StateData;
    public D_MeleeAttack attack2StateData;
    public D_DeadState deadStateData;
    public D_PlayerDetected playerDetectedData;
    public D_HealState healStateData;
    // Thêm các data khác nếu cần

    [Header("References")]
    public Transform attack1Position;
    public Transform attack2Position;
    // Thêm các reference khác nếu cần

    // Boss-specific fields
    public bool playerFound { get; set; } = false;
    public bool canDoAttack1InRange { get; set; } = false;
    public bool canDoAttack1InHitzone { get; set; } = false;
    public bool canDoAttack2InRange { get; set; } = false;
    public bool canDoAttack2InHitzone { get; set; } = false;

    // Heal system
    [Header("Heal System")]
    [SerializeField] public bool enableHealSystem = true;
    public bool isHealing { get; set; } = false;
    public bool used20PHeal { get; set; } = false;
    private float lastHealTime = 0f;
    private float lastHealth = 0f;

    // Sự kiện chết
    public UnityEvent bossDied = new UnityEvent();

    // Logic tấn công
    public void LookAtPlayer()
    {
        // Giả định playerCheck là Transform hướng về player
        if (playerCheck == null) return;
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null) return;
        Vector3 flipped = transform.localScale;
        flipped.z *= -1f;
        if (transform.position.x < player.transform.position.x && transform.localScale.x < 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(flipped.x), flipped.y, flipped.z);
        }
        else if (transform.position.x > player.transform.position.x && transform.localScale.x > 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(flipped.x), flipped.y, flipped.z);
        }
    }

    public void PerformAttack1()
    {
        // Logic tấn công 1
        // Có thể gọi animation hoặc gây damage tại đây
        Debug.Log("Boss PerformAttack1");
    }

    public void PerformAttack2()
    {
        // Logic tấn công 2
        Debug.Log("Boss PerformAttack2");
    }

    public override void Awake()
    {
        base.Awake();
        idleState = new BossIdleState(this, stateMachine, "isIdle", idleStateData, this);
        walkState = new BossWalkState(this, stateMachine, "PlayerFound", walkStateData, this);
        playerDetectedState = new BossPlayerDetectedState(this, stateMachine, "PlayerDetected", playerDetectedData, this);
        attack1State = new BossAttack1State(this, stateMachine, "Attack1", attack1Position, attack1StateData, this);
        attack2State = new BossAttack2State(this, stateMachine, "Attack2", attack2Position, attack2StateData, this);
        deadState = new BossDeadState(this, stateMachine, "Dead", deadStateData, this);
        healState = new BossHealState(this, stateMachine, "isHealing", healStateData, this);
    }

    public void Start()
    {
        stateMachine.Initialize(walkState);
        
        // Lắng nghe event khi player respawn
        GameManager.OnPlayerRespawn.AddListener(ResetBossOnPlayerRespawn);
        
        // Khởi tạo lastHealth
        lastHealth = stats.Health.CurrentValue;
    }

    private void OnDestroy()
    {
        // Cleanup event listener
        GameManager.OnPlayerRespawn.RemoveListener(ResetBossOnPlayerRespawn);
    }

    private void ResetBossOnPlayerRespawn()
    {
        // Reset boss về idle state và hồi máu
        playerFound = false;
        canDoAttack1InRange = false;
        canDoAttack1InHitzone = false;
        canDoAttack2InRange = false;
        canDoAttack2InHitzone = false;
        
        // Reset heal system
        ResetHealSystem();
        
        // Hồi máu boss về full
        var stats = Core.GetCoreComponent<Stats>();
        if (stats != null)
        {
            stats.Health.Init(); // Reset về max health
            Debug.Log("Boss health restored to full on player respawn");
        }
        
        // Chuyển về idle state
        stateMachine.ChangeState(idleState);
        Debug.Log("Boss reset to idle state on player respawn");
    }

    public override void Update()
    {
        base.Update();

        if (playerFound)
        {
            canDoAttack1InRange = CheckPlayerInMinAgroRange();
            canDoAttack2InRange = CheckPlayerInMinAgroRange();

            canDoAttack1InHitzone = CheckPlayerInCloseRangeAction();
            canDoAttack2InHitzone = CheckPlayerInCloseRangeAction();
        }
        else
        {
            canDoAttack1InRange = false;
            canDoAttack1InHitzone = false;
            canDoAttack2InRange = false;
            canDoAttack2InHitzone = false;
        }
        
        // Kiểm tra heal system
        CheckHealSystem();
    }
    
    private void CheckHealSystem()
    {
        float currentHealth = stats.Health.CurrentValue;
        
        // Kiểm tra nếu máu giảm (bị damage) và không đang heal
        if (!isHealing && currentHealth < lastHealth && enableHealSystem && Time.time - lastHealTime > healStateData.healCooldown)
        {
            CheckHealConditions();
        }
        
        lastHealth = currentHealth;
    }

    private void CheckHealConditions()
    {
        if (stats == null) return;
        
        float healthPercentage = stats.Health.CurrentValue / stats.Health.MaxValue;
        
        Debug.Log($"Boss máu: {stats.Health.CurrentValue}/{stats.Health.MaxValue} ({healthPercentage:P0})");
        
        if (healthPercentage < healStateData.healthThreshold20 && !used20PHeal) 
        {
            Debug.Log($"Boss máu thấp! Trigger heal tại {healthPercentage:P0} máu");
            TriggerHeal();
            used20PHeal = true;
        }
    }

    private void TriggerHeal()
    {
        isHealing = true;
        lastHealTime = Time.time;
        Debug.Log($"Boss bắt đầu heal! Máu hiện tại: {stats.Health.CurrentValue}/{stats.Health.MaxValue}");
        stateMachine.ChangeState(healState);
    }

    public void OnHealComplete()
    {
        isHealing = false;
        Debug.Log("Boss đã hoàn thành heal, reset trạng thái!");
    }

    private void ResetHealSystem()
    {
        isHealing = false;
        used20PHeal = false;
        lastHealTime = 0f;
    }

    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(attack1Position.position, attack1StateData.attackRadius);
        Gizmos.DrawWireSphere(attack2Position.position, attack2StateData.attackRadius);

    }
}