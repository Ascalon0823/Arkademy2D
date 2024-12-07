using UnityEngine;

namespace Arkademy.Test.Input
{
    public class DragIndicator : MonoBehaviour
    {
        [SerializeField] private Transform handleNormalized;
        [SerializeField] private Transform handleAnaglog;
        [SerializeField] private Transform handleRaw;
        public void UpdatePos(Vector3 start, Vector2 deltaNormalize, Vector2 deltaAnalog)
        {
            transform.position = start;
            handleNormalized.localPosition = deltaNormalize * 2f;
            handleAnaglog.localPosition = deltaAnalog * 2f;
        }
    }
}