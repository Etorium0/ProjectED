using Etorium.CoreSystem;
using UnityEngine;

namespace Etorium.UI
{
    public class PlayerHealthUI : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private HealthBarUI healthBarUI;
        [SerializeField] private Core playerCore;
        
        [Header("Settings")]
        [SerializeField] private bool showHealthText = true;
        [SerializeField] private bool animateHealthChanges = true;
        
        private Stats playerStats;
        
        private void Start()
        {
            if (playerCore == null)
                playerCore = FindObjectOfType<Core>();
                
            if (healthBarUI == null)
                healthBarUI = GetComponent<HealthBarUI>();
                
            if (playerCore != null)
            {
                playerStats = playerCore.GetCoreComponent<Stats>();
                
                if (playerStats != null && healthBarUI != null)
                {
                    InitializeHealthBar();
                }
            }
        }
        
        private void OnEnable()
        {
            if (playerStats != null && playerStats.Health != null)
            {
                playerStats.Health.OnValueChanged += HandleHealthChanged;
            }
        }
        
        private void OnDisable()
        {
            if (playerStats != null && playerStats.Health != null)
            {
                playerStats.Health.OnValueChanged -= HandleHealthChanged;
            }
        }
        
        private void InitializeHealthBar()
        {
            if (healthBarUI != null && playerStats != null)
            {
                healthBarUI.Initialize(playerStats.Health.MaxValue);
                healthBarUI.SetHealth(playerStats.Health.CurrentValue);
            }
        }
        
        private void HandleHealthChanged(float currentHealth, float maxHealth)
        {
            if (healthBarUI != null)
            {
                healthBarUI.SetHealth(currentHealth);
                healthBarUI.SetMaxHealth(maxHealth);
            }
        }
        
        public void SetPlayerCore(Core core)
        {
            // Unsubscribe from old stats
            if (playerStats != null && playerStats.Health != null)
            {
                playerStats.Health.OnValueChanged -= HandleHealthChanged;
            }
            
            playerCore = core;
            
            if (playerCore != null)
            {
                playerStats = playerCore.GetCoreComponent<Stats>();
                
                if (playerStats != null && healthBarUI != null)
                {
                    // Subscribe to new stats
                    playerStats.Health.OnValueChanged += HandleHealthChanged;
                    InitializeHealthBar();
                }
            }
        }
        
        public HealthBarUI GetHealthBarUI() => healthBarUI;
        public Core GetPlayerCore() => playerCore;
        public Stats GetPlayerStats() => playerStats;

        private void Update()
        {
            if (playerStats != null && healthBarUI != null)
            {
                healthBarUI.SetHealth(playerStats.Health.CurrentValue);
                healthBarUI.SetMaxHealth(playerStats.Health.MaxValue);
            }
        }
    }
} 