using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCrossSmallImpact : MonoBehaviour
    {
        public float lifeTime = 1f;
        public void Initalize(GridCell cell, SkillAbstract skill)
        {
            skill.Excute(Time.deltaTime, cell);
            Destroy(gameObject, lifeTime);
        }
    }
}