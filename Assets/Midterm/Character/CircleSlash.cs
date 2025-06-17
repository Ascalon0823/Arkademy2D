using UnityEngine;

namespace Midterm.Character
{
    public class CircleSlash : Ability
    {
        [SerializeField] protected GameObject slash;
        [SerializeField] protected Animator slashAnimator;

        protected override void Update()
        {
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
            slash.transform.localPosition = user.faceDir * 0.5f;
            slashAnimator.SetFloat("speed", 1f / useTime);
        }
    }
}