using System;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    [Serializable]
    public class AttackActionHitBox : AttackData
    {
        public bool Debug;
        
        [field: SerializeField] public Rect HitBox { get; private set; }
        
    }
}