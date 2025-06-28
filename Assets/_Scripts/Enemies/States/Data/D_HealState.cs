using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newHealStateData", menuName = "Data/State Data/Heal State")]
public class D_HealState : ScriptableObject
{
    [Header("Heal Settings")]
    public float healAmount = 1f; // 100% max health (hồi đầy)
    public float healDuration = 3f; // Thời gian heal
    public float healInterruptionTime = 1f; // Thời gian có thể bị gián đoạn
    public float healCooldown = 10f; // Cooldown giữa các lần heal
    
    [Header("Heal Triggers")]
    public float healthThreshold20 = 0.2f; // Chỉ heal khi 20% health
    
    [Header("Effects")]
    public GameObject healParticleEffect;
    public AudioClip healSound;
}