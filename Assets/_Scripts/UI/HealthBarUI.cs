using UnityEngine;
using UnityEngine.UI;

namespace Etorium.UI
{
    public class HealthBarUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image fillImage;

        [Header("Color Settings")]
        [SerializeField] private Color highHealthColor = Color.green;
        [SerializeField] private Color mediumHealthColor = Color.yellow;
        [SerializeField] private Color lowHealthColor = Color.red;
        [SerializeField] private float lowHealthThreshold = 0.3f;
        [SerializeField] private float mediumHealthThreshold = 0.6f;

        private float currentHealth;
        private float maxHealth;

        public void Initialize(float maxHealthValue)
        {
            maxHealth = maxHealthValue;
            currentHealth = maxHealthValue;
            UpdateUI();
        }

        public void SetHealth(float health)
        {
            currentHealth = Mathf.Clamp(health, 0f, maxHealth);
            UpdateUI();
        }

        public void SetMaxHealth(float maxHealthValue)
        {
            maxHealth = maxHealthValue;
            if (currentHealth > maxHealth)
                currentHealth = maxHealth;
            UpdateUI();
        }

        private void UpdateUI()
        {
            if (fillImage != null && maxHealth > 0f)
            {
                float percent = currentHealth / maxHealth;
                fillImage.fillAmount = percent;
                UpdateHealthBarColor(percent);
            }
        }

        private void UpdateHealthBarColor(float percent)
        {
            if (fillImage == null) return;

            if (percent <= lowHealthThreshold)
                fillImage.color = lowHealthColor;
            else if (percent <= mediumHealthThreshold)
                fillImage.color = mediumHealthColor;
            else
                fillImage.color = highHealthColor;
        }
    }
} 