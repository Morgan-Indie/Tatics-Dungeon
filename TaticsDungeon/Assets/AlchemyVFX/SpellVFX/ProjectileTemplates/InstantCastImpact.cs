using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class InstantCastImpact : MonoBehaviour
    {
        GridCell destination;
        public float lifeTime = 1f;

        public void Initalize(GridCell cell, SkillAbstract skill)
        {
            destination = cell;
            skill.Excute(Time.deltaTime, cell);
            Destroy(gameObject, lifeTime);
        }
    }
}