using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Midterm.Character
{
    public class SpellCastUI : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public GameObject selectionBase;
        public RectTransform[] optionsLocation;
        public List<RectTransform> currSelections = new();

        public bool preparing;
        public bool readyToPrepare;
        public Character character;

        public Image currSpellIcon;

        private void Start()
        {
            selectionBase.SetActive(false);
        }

        private void LateUpdate()
        {
            if (character.preparing && !preparing)
            {
                OnBeginPrepare();
            }

            if (preparing && !character.preparing)
            {
                OnEndPrepare();
            }

            if (preparing)
            {
                if (!readyToPrepare && character.moveDir.magnitude < 0.01f)
                {
                    readyToPrepare = true;
                }

                if (readyToPrepare)
                {
                    OnPrepare(character.moveDir);
                }
            }

            currSpellIcon.sprite = character.currSpell?.icon ?? null;
            currSpellIcon.enabled = currSpellIcon.sprite;
        }

        public void OnDrag(PointerEventData eventData)
        {
            var dir = (eventData.position - (Vector2)GetComponent<RectTransform>().position) / 32 /
                      transform.root.GetComponent<CanvasScaler>().scaleFactor;
            OnPrepare(dir);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            OnBeginPrepare();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnEndPrepare();
        }

        public void OnBeginPrepare()
        {
            foreach (RectTransform option in optionsLocation)
            {
                option.GetComponent<Image>().color = Color.grey;
            }

            selectionBase.SetActive(true);
            preparing = true;
            readyToPrepare = false;
        }

        public void OnEndPrepare()
        {
            var key = string.Join("", currSelections.Select(x => x.gameObject.name));
            Debug.Log(key);
            selectionBase.SetActive(false);
            currSelections.Clear();
            preparing = false;
            character.ChangeSpell(key);
        }

        public void OnPrepare(Vector2 dir)
        {
            if (dir.magnitude <= 0.65f) return;
            var nearest = optionsLocation.OrderByDescending(x => Vector3.Dot(
                x.localPosition.normalized, dir.normalized
            )).First();
            if (Vector3.Dot(nearest.localPosition.normalized, dir.normalized) < 0.75f)
            {
                return;
            }

            if (currSelections.Contains(nearest)) return;
            currSelections.Add(nearest);
            nearest.GetComponent<Image>().color = Color.white;
        }
    }
}