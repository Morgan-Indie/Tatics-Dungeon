using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class StatScalingStruct
    {
        public float Value;
        public StatModType Type;

        public StatScalingStruct(StatModType type, float value)
        {
            Value = value;
            Type = type;
        }

    }
}

