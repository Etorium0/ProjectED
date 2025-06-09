using System;
using Etorium.Utilities;
using UnityEngine;

namespace Etorium.Weapons
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] private int numberOfAttacks;
        [SerializeField] private float attackCounterResetCooldown;

        public int CurrentAttackCounter
        {
            get => currentAttackCounter;
            private set => currentAttackCounter = value >= numberOfAttacks ? 0 : value; 
        }
        
        public event Action OnExit;
        
        private Animator anim;
        public GameObject BaseGameObject {get; private set;}
        public GameObject WeaponSpriteGameObject {get; private set;}
        
        private AnimationEventHandler eventHandler;

        private int currentAttackCounter;

        private Timer attackCounterResetTimer;
        
        public void Enter()
        {
            print($"{transform.name} enter");
            
            attackCounterResetTimer.StopTimer();
            
            anim.SetBool("active", true);
            anim.SetInteger("counter", CurrentAttackCounter);
        }

        private void Exit()
        {
            anim.SetBool("active", false);
            
            CurrentAttackCounter++;
            attackCounterResetTimer.StartTimer();
            
            OnExit?.Invoke();
        }

        private void Awake()
        {
            BaseGameObject = transform.Find("Base").gameObject;
            WeaponSpriteGameObject = transform.Find("WeaponSprite").gameObject;
            anim = BaseGameObject.GetComponent<Animator>();

            eventHandler = BaseGameObject.GetComponent<AnimationEventHandler>();

            attackCounterResetTimer = new Timer(attackCounterResetCooldown);
        }

        public void Update()
        {
            attackCounterResetTimer.Tick();
        }

        private void ResetAtackCounter() => CurrentAttackCounter = 0;

        private void OnEnable()
        {
            eventHandler.OnFinish += Exit;
            attackCounterResetTimer.OnTimerDone += ResetAtackCounter;
        }

        private void OnDisable()
        {
            eventHandler.OnFinish -= Exit;
            attackCounterResetTimer.OnTimerDone -= ResetAtackCounter;

        }
    }
}