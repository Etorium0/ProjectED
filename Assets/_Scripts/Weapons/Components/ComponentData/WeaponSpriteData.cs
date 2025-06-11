using Etorium.Weapons.Components.AttackData;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    public class WeaponSpriteData: ComponentData
    {
        [field: SerializeField] public AttackSprites[] AttackData {get; private set;}
    }
}