using System;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    public class WeaponSprite : WeaponComponent
    {
        private SpriteRenderer baseSpriteRenderer;
        private SpriteRenderer weaponSpriteRenderer;

        [SerializeField] private WeaponSprites[] weaponSprites;

        protected override void Awake()
        {
            base.Awake();
            //TODO: Fix this when have weapon data (WeaponSprite phụ thuộc vào Weapon để lấy gameObject nên nếu awake này được gọi trước weapon.Awake sẽ lõi nullrefer
            // baseSpriteRenderer = weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            // weaponSpriteRenderer = weapon.WeaponSpriteGameObject.GetComponent<SpriteRenderer>();
        }
    }
    
    [Serializable] 

    public class WeaponSprites
    {
        [field: SerializeField] public Sprite[] Sprites { get; private set;}
    }
}