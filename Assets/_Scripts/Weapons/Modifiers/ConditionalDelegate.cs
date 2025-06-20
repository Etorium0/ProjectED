using Etorium.Weapons.Components;
using UnityEngine;

namespace Etorium.Weapons.Modifiers
{
    public delegate bool ConditionalDelegate(Transform source, out DirectionalInformation directionalInformation);
}