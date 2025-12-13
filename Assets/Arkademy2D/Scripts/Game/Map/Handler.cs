using System.Collections;
using System.Collections.Generic;
using Arkademy2D.Game.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Arkademy2D.Game.Map
{
    public class Handler : MonoBehaviour
    {
        public List<TileMap> maps;
        public Image transition;
        public void Load(string mapName)
        {
            StopAllCoroutines();
            StartCoroutine(Transition(mapName));
        }

        IEnumerator Transition(string mapName)
        {
            while (transition.color.a < 1)
            {
                transition.color += new Color(0,0,0,Time.deltaTime*5f);
                yield return null;
            }
            foreach (var map in maps)
            {
                map.gameObject.SetActive(map.name == mapName);
                if (map.gameObject.activeInHierarchy)
                {
                    map.onPlayerEnter?.Invoke();
                    FindFirstObjectByType<InputHandler>().actorMovement.body.position = map.entry.position;
                }
            }
            while (transition.color.a > 0)
            {
                transition.color -= new Color(0,0,0,Time.deltaTime*5f);
                yield return null;
            }
        }
    }
}
