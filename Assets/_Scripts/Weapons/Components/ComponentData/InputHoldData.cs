namespace Etorium.Weapons.Components
{
    public class InputHoldData : ComponentData
    {
        protected override void SetComponentDependency()
        {
            ComponentDependency = typeof(InputHold);
        }
    }
}