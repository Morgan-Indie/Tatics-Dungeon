using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FireStormImpact : MonoBehaviour
    {
        public void Initalize(GridCell cell, SkillAbstract skill)
        {
            skill.Excute(Time.deltaTime, cell);
            Destroy(gameObject, 1f);
        }
    }
}
