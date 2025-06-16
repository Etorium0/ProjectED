using Etorium.Weapons.Components;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    public class WeaponSpriteData : ComponentData<AttackSprites>
    {
        public WeaponSpriteData()
        {
            ComponentDependency = typeof(WeaponSprite);
        }
    }
}