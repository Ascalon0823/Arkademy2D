using UnityEngine;

namespace Arkademy.Gameplay
{
    public class Spawner : MonoBehaviour
    {
        public GameObject prefab;
        
        public void Spawn()
        {
            Instantiate(prefab, transform.position, Quaternion.identity);
        }
    }
}