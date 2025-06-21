﻿using System;
using Etorium.ObjectPoolSystem;
using Etorium.ProjectileSystem;
using Etorium.Weapons.Components;
using UnityEngine;

namespace Etorium.Weapons
{
    public interface IProjectileSpawnerStrategy
    {
        void SpawnProjectile(ProjectileSpawnInfo projectileSpawnInfo, Vector3 spawnerPos, int facingDirection,
            ObjectPools objectPools, Action<Projectile> OnSpawnProjectile);
        
        
    }
}