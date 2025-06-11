using System;
using UnityEngine;

namespace Etorium.Weapons.Components.AttackData
{
    [Serializable]
    public class AttackMovement
    {
        [field: SerializeField] public Vector2 Direction {get; private set;}
        [field: SerializeField] public float Velocity {get; private set;}
    }
}