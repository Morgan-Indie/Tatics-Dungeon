using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public abstract class VFXSpawns : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public float lifeTime = 1f;

        public abstract void Initialize(List<GridCell> cells, SkillAbstract skill, IntVector2 origin);
    }
}


