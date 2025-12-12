using TMPro;
using UnityEngine;

namespace Arkademy2D.Title.UI
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class VersionText : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<TextMeshProUGUI>().text = Application.version;
        }
    }
}