using Etorium.Combat.PoiseDamage;
 using Etorium.ModifierSystem;

 namespace Etorium.CoreSystem
{
    public class PoiseDamageReceiver : CoreComponent, IPoiseDamageable
    {
        private Stats stats;

        public Modifiers<Modifier<PoiseDamageData>, PoiseDamageData> Modifiers { get; } = new();

        public void DamagePoise(PoiseDamageData data)
        {
            data = Modifiers.ApplyAllModifiers(data);
            
            stats.Poise.Decrease(data.Amount);
        }

        protected override void Awake()
        {
            base.Awake();

            stats = core.GetCoreComponent<Stats>();
        }
    }
}