using UnityEngine;

public class Combat : CoreComponent, IDamageable
{
    public void Damage(float amount)
    {
        Debug.Log(core.transform.parent.name + " took " + amount + " damage.");
    }
}
