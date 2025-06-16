using Etorium.Weapons.Components;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    public class MovementData : ComponentData<AttackMovement>
    {
        public MovementData()
        {
            ComponentDependency = typeof(Movement);
        }
    }
}