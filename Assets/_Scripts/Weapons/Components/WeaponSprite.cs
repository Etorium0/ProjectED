﻿using System;
using Etorium.Weapons.Components;
using UnityEngine;

namespace Etorium.Weapons.Components
{
    public class WeaponSprite : WeaponComponent<WeaponSpriteData, AttackSprites>
    {
        private SpriteRenderer baseSpriteRenderer;
        private SpriteRenderer weaponSpriteRenderer;
        
        private int currentWeaponSpriteIndex;
        
        protected override void HandleEnter()
        {
            base.HandleEnter();
            
            currentWeaponSpriteIndex = 0;
        }
        
        private void HandleBaseSpriteChange(SpriteRenderer sr)
        {
            if (!isAttackActive)
            {
                weaponSpriteRenderer.sprite = null;
                return;
            }

            var currentAttackSprite = currentAttackData.Sprites;

            if (currentWeaponSpriteIndex >= currentAttackSprite.Length)
            {
                Debug.LogWarning($"{weapon.name} weapon sprites length mismatch");
                return;
            }
            
            weaponSpriteRenderer.sprite = currentAttackSprite[currentWeaponSpriteIndex];

            currentWeaponSpriteIndex++;
        }

        protected override void Awake()
        {
            base.Awake();
            
            baseSpriteRenderer = transform.Find("Base").GetComponent<SpriteRenderer>();
            weaponSpriteRenderer = transform.Find("WeaponSprite").GetComponent<SpriteRenderer>();
            
            // TODO: Fix this when have weapon data (WeaponSprite phụ thuộc vào Weapon để lấy gameObject nên nếu awake này được gọi trước weapon.Awake sẽ lõi nullrefer
            // baseSpriteRenderer = weapon.BaseGameObject.GetComponent<SpriteRenderer>();
            // weaponSpriteRenderer = weapon.WeaponSpriteGameObject.GetComponent<SpriteRenderer>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            
            baseSpriteRenderer.RegisterSpriteChangeCallback(HandleBaseSpriteChange);
            
            weapon.OnEnter += HandleEnter;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            baseSpriteRenderer.UnregisterSpriteChangeCallback(HandleBaseSpriteChange);
            
            weapon.OnEnter -= HandleEnter;
        }
    }
}