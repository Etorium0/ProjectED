﻿using Etorium.Weapons.Components;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    public class Movement : WeaponComponent<MovementData, AttackMovement>
    {
        private CoreSystem.Movement coreMovement;

        private CoreSystem.Movement CoreMovement =>
            coreMovement ? coreMovement : Core.GetCoreComponent(ref coreMovement);
        
        private void HandleStartMovement()
        {
            CoreMovement.SetVelocity(currentAttackData.Velocity, currentAttackData.Direction, CoreMovement.FacingDirection);
        }

        private void HandleStopMovement()
        {
            CoreMovement.SetVelocityZero();
        }

        protected override void Start()
        {
            base.Start();

            EventHandler.OnStartMovement += HandleStartMovement;
            EventHandler.OnStopMovement += HandleStopMovement;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            EventHandler.OnStartMovement -= HandleStartMovement;
            EventHandler.OnStopMovement -= HandleStopMovement;
        }
    }
}