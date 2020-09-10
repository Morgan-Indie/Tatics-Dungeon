using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCircleSpawn : MonoBehaviour
    {
        public float lifeTime = 1f;

        public void Initalize(GridCell _cell, SkillAbstract _skill)
        {
            _skill.Excute(Time.deltaTime, _cell);
            Destroy(gameObject, lifeTime);
        }
    }
}