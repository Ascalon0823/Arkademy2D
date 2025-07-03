using UnityEngine;

namespace Midterm.Character
{
    public class DestroyOnAnimEvent : MonoBehaviour
    {
        public void OnDestroyAnimEvent()
        {
            Destroy(transform.root.gameObject);
        }
    }
}