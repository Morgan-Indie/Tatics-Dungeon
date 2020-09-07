using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class DamageStruct
    {
        public int poison;
        public int pierce;
        public int normal;
        public int shock;
        public int fire;
        public int water;

        public DamageStruct(float fireDamage,float waterDamage, float poisonDamage,float shockDamage, float pierceDamage, float normalDamage)
        {
            poison = (int)poisonDamage;
            pierce = (int)pierceDamage;
            normal = (int)normalDamage;
            fire = (int)fireDamage;
            water = (int)waterDamage;
            shock = (int)shockDamage;
        }

        public DamageStruct() : this(0f, 0f, 0f, 0f, 0f, 0f) { }
    }
}

