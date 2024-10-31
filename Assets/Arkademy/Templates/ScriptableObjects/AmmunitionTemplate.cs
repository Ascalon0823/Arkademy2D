using Arkademy.Data;
using UnityEngine;

namespace Arkademy.Templates
{
    [CreateAssetMenu(fileName = "New Ammunition", menuName = "Template/Item/Ammunition", order = 0)]
    public class AmmunitionTemplate : ItemTemplate<Ammunition>
    {
        public Behaviour.Projectile projectilePrefab;

        public Behaviour.Projectile GetAmmuBehaviour()
        {
            return Instantiate(projectilePrefab);
        }
    }
}