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
<<<<<<< Updated upstream
        public int shock;
        public int fire;
        public int water;

        public DamageStruct(float fireDamage,float waterDamage, float poisonDamage,float shockDamage, float pierceDamage, float normalDamage)
=======
        public DamageStatType alchemicalType;

        public DamageStruct(float pierceDamage, float normalDamage, float alchemicalDamage, DamageStatType _alchemicalType)
>>>>>>> Stashed changes
        {
            poison = (int)poisonDamage;
            pierce = (int)pierceDamage;
            normal = (int)normalDamage;
            fire = (int)fireDamage;
            water = (int)waterDamage;
            shock = (int)shockDamage;
        }

<<<<<<< Updated upstream
        public DamageStruct() : this(0f, 0f, 0f, 0f, 0f, 0f) { }
=======
        public DamageStruct() : this(0f, 0f, 0f, DamageStatType.normalDamage) { }
>>>>>>> Stashed changes
    }
}

