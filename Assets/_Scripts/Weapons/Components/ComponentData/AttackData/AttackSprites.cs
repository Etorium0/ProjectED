using System;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    [Serializable]
    public class AttackSprites : AttackData
    {
        [field: SerializeField] public Sprite[] Sprites {get; private set;}
        
        
    }
}