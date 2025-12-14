using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Arkademy2D.Core.Data;
using Arkademy2D.Core.Models;
using Arkademy2D.Core.System;
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
            var academicRecord = GameSystem.Character.CharacterModel.AcademicRecord;
            var modules = ModuleData.Modules;
            foreach (var module in modules)
            {
                var go = moduleObjects.First(x => x.name == module.DisplayName);
                SetupGO(academicRecord, module, go);
            }
        }
        private void OnDisable()
        {
            EventSystem.current.SetSelectedGameObject(null);
            FindFirstObjectByType<PlayerInput>().SwitchCurrentActionMap("Player");
        }

        private void SetupGO(AcademicRecord data, ModuleData module, GameObject go)
        {
            var studied = data.ModuleProgress.TryGetValue(module.DisplayName, out var progress);
            var button = go.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            var text = go.GetComponentInChildren<TextMeshProUGUI>();
            if (moduleObjects[0] == go)
            {
                button.Select();
            }

            if (studied)
            {
                text.text = $"{module.DisplayName} {progress}";
            }

            button.onClick.AddListener(() =>
            {
                if (progress == 100)
                {
                    return;
                }

                if (!studied)
                {
                    if (module.Prerequisites.Count > 0 &&
                        module.Prerequisites.Any(x => !data.ModuleProgress.TryGetValue(x, out var p) || p < 100))
                    {
                        return;
                    }
                }
                data.ModuleProgress[module.DisplayName] = studied ? progress + 20 : 0;
                UpdatePage();
            });
        }
    }
}