using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class DamageStruct
    {
        public int elemental;
        public int pierce;
        public int normal;

        public DamageStruct(float elementalDamage, float pierceDamage, float normalDamage)
        {
            elemental = (int)elementalDamage;
            pierce = (int)pierceDamage;
            normal = (int)normalDamage;
        }
    }
}

