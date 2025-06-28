using System;
using UnityEngine;

namespace Etorium.CoreSystem.StatsSystem
{
    [Serializable]
    public class Stat
    {
        public event Action OnCurrentValueZero;
        public event Action<float, float> OnValueChanged; // currentValue, maxValue
        
        [field: SerializeField] public float MaxValue { get; private set; }

        public float CurrentValue
        {
            get => currentValue;
            set
            {
                float oldValue = currentValue;
                currentValue = Mathf.Clamp(value, 0f, MaxValue);

                if (currentValue <= 0f)
                {
                    OnCurrentValueZero?.Invoke();
                }
                
                // Trigger value changed event if value actually changed
                if (oldValue != currentValue)
                {
                    OnValueChanged?.Invoke(currentValue, MaxValue);
                }
            }
        }
        
        private float currentValue;

        public void Init() => CurrentValue = MaxValue;

        public void Increase(float amount) => CurrentValue += amount;

        public void Decrease(float amount) 
        {
            CurrentValue -= amount;
            Debug.Log("CurrentValue: " + CurrentValue);
        }
    }
}