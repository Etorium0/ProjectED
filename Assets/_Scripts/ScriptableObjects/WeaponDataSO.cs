using System.Collections.Generic;
using System.Linq;
using Etorium.Weapons.Components.ComponentData;
using Etorium.Weapons.Components.ComponentData.AttackData;

using UnityEngine;

namespace Etorium.Weapons
{
    [CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Basic Weapon Data", order = 0)]
    public class WeaponDataSO : ScriptableObject
    {
        [field: SerializeField] public int numberOfAttacks {get; private set;}
        
        [field: SerializeReference] public List<ComponentData> componentData {get; private set;}

        public T GetData<T>()
        {
            return componentData.OfType<T>().FirstOrDefault();
        }
        
        [ContextMenu("Add Sprite Data")]
        private void AddSpriteData() => componentData.Add(new WeaponSpriteData());
    }
}