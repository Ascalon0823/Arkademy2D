using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.UI
{
    public class CharacterMenu : MonoBehaviour
    {
        
        private static CharacterMenu _instance;
        
        public Page[] pages;
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
            currPage = currPage<0 ? pages.Length - 1 : currPage; 
            TogglePage(currPage);
        }

        public void NextPage()
        {
            currPage++;
            currPage = currPage > pages.Length - 1 ? 0 : currPage;
            TogglePage(currPage);
        }

        public void TogglePage(int idx)
        {
            if(pages==null) return;
            for (var i = 0; i < pages.Length; i++)
            {
                pages[i].Toggle(i==idx);
            }
        }

    }
}