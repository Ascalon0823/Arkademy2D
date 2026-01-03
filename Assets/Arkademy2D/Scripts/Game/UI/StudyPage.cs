using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Game.Data.Static.Academic;
using Arkademy2D.Core.Models;
using Arkademy2D.Game.System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Arkademy2D.Game.UI
{
    public class StudyPage : MonoBehaviour
    {
        public static void SetOpen(bool open)
        {
            _instance.gameObject.SetActive(open);
        }

        private static StudyPage _instance;
        public List<GameObject> moduleObjects = new List<GameObject>();

        private void Awake()
        {
            _instance = this;
            SetOpen(false);
        }

        private void OnEnable()
        {
            FindFirstObjectByType<PlayerInput>().SwitchCurrentActionMap("UI");
            StartCoroutine(SelectButton());
        }

        IEnumerator SelectButton()
        {
            yield return null;
           UpdatePage();
        }

        private void UpdatePage()
        {
            var academicRecord = GameSystem.CharacterModel.AcademicRecord;
            var modules = ModuleBase.Library.Values.ToList();
            foreach (var module in modules)
            {
                var go = moduleObjects.First(x => x.name == module.Id);
                SetupGO(academicRecord, module, go);
            }
        }
        private void OnDisable()
        {
            EventSystem.current?.SetSelectedGameObject(null);
            FindFirstObjectByType<PlayerInput>()?.SwitchCurrentActionMap("Player");
        }

        private void SetupGO(AcademicRecord data, ModuleBase module, GameObject go)
        {
            Debug.Log($"Setup go: {go.name} {module}");
            var studied = data.ModuleProgress.TryGetValue(module.Id, out var progress);
            var button = go.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            if (moduleObjects[0] == go)
            {
                button.Select();
            }

            if (studied)
            {
               
            }
            text.text = module.displayName + (studied?$" {progress}" : "");
            button.onClick.AddListener(() =>
            {
                if (progress == 100)
                {
                    return;
                }

                if (!studied)
                {
                    if (!AcademicSystem.ModuleAvailable(module,data))
                    {
                        return;
                    }
                }
                data.ModuleProgress[module.Id] = studied ? progress + 20 : 0;
                UpdatePage();
            });
        }
    }
}