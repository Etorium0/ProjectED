using System;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    [Serializable]
    public class ComponentData
    {
        
    }

    [Serializable]
    public class ComponentData<T> : ComponentData where T : AttackData
    {
        [field: SerializeField] public T[] AttackData { get; private set; }
    }
}