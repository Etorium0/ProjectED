using System;
using UnityEngine;

namespace Etorium.CoreSystem.StatsSystem
{
    [Serializable]
    public class Stat
    {
        public event Action OnCurrentValueZero;
        
        [field: SerializeField] public float MaxValue { get; private set; }

        public float CurrentValue
        {
            get => currentValue;
            set
            {
                currentValue = Mathf.Clamp(value, 0, MaxValue);

                if (currentValue <= 0f)
                {
                    OnCurrentValueZero?.Invoke();
                }
            }

        }
        
        private float currentValue;
        
        public void Init() => CurrentValue = MaxValue;

        public void Increase(float amount) => CurrentValue += amount;

        public void Decrease(float amount)
        {
            Debug.Log("Current Health: " + CurrentValue);

            CurrentValue -= amount;
        }
    }
}