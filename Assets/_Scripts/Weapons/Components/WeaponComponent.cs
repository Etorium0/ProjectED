using System;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    public abstract class WeaponComponent : MonoBehaviour
    {
        protected Weapon weapon;

        protected virtual void Awake()
        {
            weapon = GetComponent<Weapon>();
        }
    }
}