using System;
using Etorium.ProjectileSystem.Components;
using UnityEngine;

namespace Etorium.ProjectileSystem
{
    /*
     * This MonoBehaviour is simply used to print the damage amount received in the ProjectileTestScene
     */
    public class TestDamageable : MonoBehaviour, IDamageable
    {
        public void Damage(float amount)
        {
            print($"{gameObject.name} Damaged: {amount}");
        }
    }
}