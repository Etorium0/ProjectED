using Etorium.Weapons.Components.AttackData;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    public class MovementData : ComponentData
    {
        [field: SerializeField] public AttackMovement[] AttackData {get; private set;}
    }
}