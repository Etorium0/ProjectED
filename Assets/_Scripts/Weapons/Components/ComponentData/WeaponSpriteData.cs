using Etorium.Weapons.Components.ComponentData.AttackData;
using UnityEngine;

namespace Etorium.Weapons.Components.ComponentData
{
    public class WeaponSpriteData: ComponentData
    {
        [field: SerializeField] public AttackSprites[] AttackData {get; private set;}
    }
}