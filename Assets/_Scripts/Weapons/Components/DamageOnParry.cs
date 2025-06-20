using Etorium.Combat.Damage;
using UnityEngine;
using static Etorium.Utilities.CombatDamageUtilities;

namespace Etorium.Weapons.Components
{
    public class DamageOnParry : WeaponComponent<DamageOnParryData, AttackDamage>
    {
        private Parry parry;
        
        private void HandleParry(GameObject parriedGameObject)
        {
            TryDamage(
                parriedGameObject,
                new Combat.Damage.DamageData(currentAttackData.Amount, Core.Root),
                out _
            );
        }

        protected override void Start()
        {
            base.Start();
            
            parry = GetComponent<Parry>();

            parry.OnParry += HandleParry;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            parry.OnParry -= HandleParry;
        }
    }
}