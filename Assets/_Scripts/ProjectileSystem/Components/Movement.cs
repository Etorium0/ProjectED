using UnityEngine;

namespace Etorium.ProjectileSystem.Components
{
    public class Movement : ProjectileComponent
    {
        [field: SerializeField] public bool ApplyContinuously { get; private set; }
        [field: SerializeField] public float Speed { get; private set; }

        protected override void Init()
        {
            base.Init();

            SetVelocity();
        }

        private void SetVelocity() => rb.linearVelocity = Speed * transform.right;

        private void FixedUpdate()
        {
            if (!ApplyContinuously)
                return;
            
            SetVelocity();
        }
    }
}