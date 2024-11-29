using System;
using Arkademy.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy.Campus
{
    public class ActivityItem : MonoBehaviour
    {
        [SerializeField] Activity.Type activityType;
        [SerializeField] private Button button;
        private Activity _activity;

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void Start()
        {
            Session.currCharacterRecord.time.OnGameTimeChanged += OnGameTimeChanged;
            OnGameTimeChanged(Session.currCharacterRecord.time);
        }

        private void OnDestroy()
        {
            Session.currCharacterRecord.time.OnGameTimeChanged -= OnGameTimeChanged;
        }

        private void OnGameTimeChanged(GameTime time)
        {
            var chara = Session.currCharacterRecord.character;
            _activity = Activity.SelectActivity(chara, activityType);

            button.interactable = chara.energy.currValue >= _activity.energyCost
                                  && 16 - Session.currCharacterRecord.time.hour >= _activity.timeCost;
        }

        public void OnClick()
        {
            _activity.TakeActivity();
        }
    }
}