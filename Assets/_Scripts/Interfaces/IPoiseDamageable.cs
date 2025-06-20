using Etorium.Combat.PoiseDamage;

namespace Etorium.Interfaces
{
    public interface IPoiseDamageable
    {
        void DamagePoise(PoiseDamageData amount);
    }
}