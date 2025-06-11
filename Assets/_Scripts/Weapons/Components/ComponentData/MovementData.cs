using Etorium.Weapons.Components.ComponentData.AttackData;
using UnityEngine;

namespace Etorium.Weapons.Components.ComponentData
{
    public class MovementData : ComponentData
    {
        [field: SerializeField] public AttackMovement[] AttackData {get; private set;}
    }
}