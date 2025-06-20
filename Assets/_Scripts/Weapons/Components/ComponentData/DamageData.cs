namespace Etorium.Weapons.Components
{
    public class DamageData : ComponentData<AttackDamage>
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(Damage);
        }
    }
}