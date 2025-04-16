using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.UI
{
    public class CharacterMenu : MonoBehaviour
    {
        
        private static CharacterMenu _instance;
        
        public CanvasGroup[] canvasGroups;
        public int currPage;
        private void Awake()
        {
            if (_instance && _instance != this)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            Toggle(false);
        }

        public static void Show()
        {
            _instance.Toggle(true);
        }

        public void Toggle(bool active)
        {
            gameObject.SetActive(active);
            if (active)
            {
                Setup();
            }
        }

        public void Setup()
        {
            currPage = 0;
            TogglePage(currPage);
        }

        public void PrevPage()
        {
            currPage--;
            currPage = currPage<0 ? canvasGroups.Length - 1 : currPage; 
            TogglePage(currPage);
        }

        public void NextPage()
        {
            currPage++;
            currPage = currPage > canvasGroups.Length - 1 ? 0 : currPage;
            TogglePage(currPage);
        }

        public void TogglePage(int idx)
        {
            if(canvasGroups==null) return;
            for (var i = 0; i < canvasGroups.Length; i++)
            {
                canvasGroups[i].alpha = idx == i ? 1 : 0;
                canvasGroups[i].interactable = idx == i;
                canvasGroups[i].blocksRaycasts = idx == i;
            }
        }

    }
}