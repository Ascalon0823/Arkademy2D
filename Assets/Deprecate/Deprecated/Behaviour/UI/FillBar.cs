using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Behaviour.UI
{
    [ExecuteInEditMode]
    public class FillBar : MonoBehaviour
    {
        public Gradient colorGradient;
        [Range(0,1)]
        public float value;
        public Image fill;
        
        private void LateUpdate()
        {
            fill.color = colorGradient.Evaluate(value);
            fill.fillAmount = value;
        }
    }

}
