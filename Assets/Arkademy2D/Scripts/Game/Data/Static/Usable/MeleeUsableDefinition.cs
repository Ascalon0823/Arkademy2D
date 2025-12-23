using Arkademy2D.Game.Behaviours.Actor;
using Arkademy2D.Game.Data.Runtime;
using UnityEngine;
namespace Arkademy2D.Game.Data.Static.Usable
{
    [CreateAssetMenu(fileName = "Melee Usable Effect", menuName = "Static/Usable/Melee")]
    public class MeleeUsableDefinition : UsableEffectDefinition
    {
        public float damageMultiplier = 1;
        public float rangeMultiplier = 1;
        public override void UseEffect(UseContext context)
        {
            var finalDamage = Mathf.FloorToInt(damageMultiplier * context.meleeContext.Value.Power);
            var finalRange = rangeMultiplier * context.meleeContext.Value.Range;
            var pos = (Vector2)context.userTransform.position + context.character.movement.faceDir.normalized * finalRange / 2f;
            var angle = Vector2.SignedAngle(Vector2.up, context.point);
            var hits = Physics2D.OverlapBoxAll(pos, Vector2.one * finalRange, angle);
            DrawBox(pos,Vector2.one * finalRange,angle,Color.red);
            foreach (var hit in hits)
            {
                var health = hit.GetComponent<Health>();
                if (!health || health.faction == context.character.health.faction)
                {
                    continue;
                }
                health.TakeDamage(finalDamage);
            }
        }

        private static void DrawBox(Vector2 pos, Vector2 size, float angle, Color color)
        {
            var p0 = - size / 2f;
            var p2 = size / 2;
            var p1 = new Vector2(-size.x, size.y) / 2f;
            var p3 = new Vector2(size.x, -size.y) / 2f;
            var rot =  Quaternion.Euler(0, 0, angle);
            p0 = rot * p0;
            p2 = rot * p2;
            p1 = rot * p1;
            p3 = rot * p3;
            p0+=pos;
            p2+=pos;
            p1+=pos;
            p3+=pos;
            Debug.DrawLine(p0, p1,color,5f);
            Debug.DrawLine(p1, p2,color,5f);
            Debug.DrawLine(p2, p3,color,5f);
            Debug.DrawLine(p3, p0,color,5f);
            Debug.DrawLine(p0,p2,color,5f);
            Debug.DrawLine(p3,p1,color,5f);
        }
    }
}