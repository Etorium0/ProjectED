using UnityEngine;

namespace Etorium.Weapons.Components
{
    public class ActionHitBoxData : ComponentData<AttackActionHitBox>
    {
        [field: SerializeField] public LayerMask DetectedLayers { get; private set; }

        public ActionHitBoxData()
        {
            ComponentDependency = typeof(ActionHitBox);
        }
    }
}