using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class IceBombImpact : MonoBehaviour
    {
        public void Initalize(GridCell cell, SkillAbstract skill)
        {
            skill.Excute(Time.deltaTime, cell);
            //AlchemyManager.Instance.ApplyChill(cell);
            Destroy(gameObject, 0.75f);
        }
    }
}