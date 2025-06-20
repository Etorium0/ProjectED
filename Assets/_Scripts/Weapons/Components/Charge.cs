using Etorium.Utilities;

namespace Etorium.Weapons.Components
{
    public class Charge : WeaponComponent<ChargeData, AttackCharge>
    {
        private int currentCharge;

        private TimeNotifier timeNotifier;
        
        protected override void HandleEnter()
        {
            base.HandleEnter();

            currentCharge = currentAttackData.InitialChargeAmount;
        }

        #region Plumbing

        

        #endregion
    }
}