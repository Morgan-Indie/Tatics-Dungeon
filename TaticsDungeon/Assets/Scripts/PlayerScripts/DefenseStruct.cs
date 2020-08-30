using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class DefenseStruct
    {
        public int resistance;
        public int normal;

        public DefenseStruct(float normalDefense, float resistance)
        {
            resistance = (int)resistance;
            normal = (int)normalDefense;
        }
    }
}