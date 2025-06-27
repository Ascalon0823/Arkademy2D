using UnityEngine;

namespace Midterm.Character
{
    public class CircleSlash : Ability
    {
        [SerializeField] protected GameObject slash;
        [SerializeField] protected Animator slashAnimator;
        [SerializeField] protected DamageTrigger damageTrigger;

        protected override void Update()
        {
            damageTrigger.damage = Mathf.FloorToInt(100 * user.power * (1 + currLevel/2f));
            base.Update();
            if (remainingUseTime <= 0)
            {
                slash.gameObject.SetActive(false);
            }
        }

        public override void Use()
        {
            base.Use();
            slash.SetActive(true);
            slash.transform.localPosition = user.faceDir * 0.5f * (1 + currLevel/4f);
            slash.transform.localScale = Vector3.one * (1 + currLevel/4f);
            slashAnimator.SetFloat("speed", 1f / GetUseTime());
        }
    }
}