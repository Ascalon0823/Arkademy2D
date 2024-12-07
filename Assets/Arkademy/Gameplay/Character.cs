using Arkademy.Behaviour;
using Arkademy.CharacterCreation;
using UnityEngine;

namespace Arkademy.Gameplay
{
    public class Character : MonoBehaviour
    {
        public Common.Character characterData;
        public CharacterGraphic graphic;

        public static Character Create(Common.Character data)
        {
            var raceName = data.raceName;
            var raceSO = Resources.Load<Race>(raceName);
            if (!raceSO || !raceSO.behaviourPrefab) return null;
            var go = Instantiate(raceSO.behaviourPrefab);
            go.characterData = data;
            go.graphic.animator.runtimeAnimatorController = raceSO.animationController;
            go.graphic.facingLeft = !raceSO.facingRight;
            go.graphic.walkAnimationDistance = 4;
            return go;
        }
    }
}