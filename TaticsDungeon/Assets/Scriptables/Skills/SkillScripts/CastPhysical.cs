using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public abstract class CastPhysical : SkillAbstract
    {
        public float scaleValue;
        public abstract void Excute(float delta);
    }
}