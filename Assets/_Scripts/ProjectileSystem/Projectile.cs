using System;
using UnityEngine;

namespace Etorium.ProjectileSystem
{
    public class Projectile : MonoBehaviour
    {
        public Rigidbody2D rigidbody2d {get; private set;}

        private void Awake()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
        }
    }
}