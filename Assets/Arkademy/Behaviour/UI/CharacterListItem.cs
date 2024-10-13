using System;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Behaviour.UI
{
    public class CharacterListItem : MonoBehaviour
    {
        public CharacterList list;
        public CharacterRecord record;
        public bool selected;
        public GameObject selectionHighlight;
        public GameObject addSign;

        private void Awake()
        {
            selectionHighlight.gameObject.SetActive(false);
        }

        public void Setup(CharacterRecord setupRecord)
        {
            record = setupRecord;
            addSign.gameObject.SetActive(false);
        }

        public void Select(bool isSelected)
        {
            if (record == null)
            {
                return;
            }

            selected = isSelected;
            selectionHighlight.gameObject.SetActive(isSelected);
        }

        public void OnClick()
        {
            if (record == null)
            {
                //Add item
            }

            list.SelectItem(this);
        }
    }
}