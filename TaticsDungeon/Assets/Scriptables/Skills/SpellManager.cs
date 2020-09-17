using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class SpellManager : MonoBehaviour
    {
        public static SpellManager Instance;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public GameObject BuildSpellPrefab(GameObject prefab, Vector3 pos)
        {
            return Instantiate(prefab, pos, Quaternion.identity);
        }
    }
}
