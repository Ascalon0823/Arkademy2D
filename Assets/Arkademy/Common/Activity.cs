using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Arkademy.Common
{
    
    [Serializable]
    public class Activity
    {
        public enum Type
        {
            Sleep,
            Rest,
            Exercise
        }
        public int timeCost;
        public int energyCost;
        public Character participant;
        [JsonIgnore]public Action<Character> OnTakeActivity;

        public void TakeActivity()
        {
            OnTakeActivity?.Invoke(participant);
            participant.energy.currValue -= energyCost;
            participant.energy.currValue = Mathf.Clamp(participant.energy.currValue, 0, participant.energy.value);
            Session.currCharacterRecord.time.AddHour(timeCost);
        }

        public static Activity SelectActivity(Character character, Activity.Type activityType)
        {
            switch (activityType)
            {
                case Type.Sleep:
                    return Sleep(character);
                case Type.Rest:
                    return Rest(character);
                case Type.Exercise:
                    return Exercise(character);
                default:
                    throw new ArgumentOutOfRangeException(nameof(activityType), activityType, null);
            }
        }
        public static Activity Sleep(Character character)
        {
            return new Activity
            {
                timeCost = 16 - Session.currCharacterRecord.time.hour,
                energyCost = -(character.energy.value - character.energy.currValue),
                participant = character
            };
        }

        public static Activity Rest(Character character)
        {
            return new Activity
            {
                timeCost = 1,
                energyCost = -15,
                participant = character
            };
        }

        public static Activity Exercise(Character character)
        {
            return new Activity
            {
                timeCost = 2,
                energyCost = 20,
                participant = character,
                OnTakeActivity = c =>
                {
                    c.energy.value += 1;
                }
            };
        }
    }
}