using System;
using UnityEngine;

namespace Etorium.ProjectileSystem.Components
{
    public class ProjectileComponent : MonoBehaviour
    {
        // Base class for any projectile components to implement repeated functionality
        protected Projectile projectile;

        protected Rigidbody2D rb => projectile.rigidbody2d;

        protected virtual void Init()
        {
            
        }

        protected void Awake()
        {
            projectile = GetComponentInChildren<Projectile>();
        }
    }
}