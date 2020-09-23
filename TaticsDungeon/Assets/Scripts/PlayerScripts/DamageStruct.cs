using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class DamageStruct
    {
        public int alchemical;
        public int pierce;
        public int normal;
        public CombatStatType alchemicalType;

        public DamageStruct(float pierceDamage, float normalDamage, float alchemicalDamage, CombatStatType _alchemicalType)
        {
            alchemical = (int)alchemicalDamage;
            pierce = (int)pierceDamage;
            normal = (int)normalDamage;
            alchemicalType = _alchemicalType;
        }

        public DamageStruct() : this(0f, 0f, 0f, CombatStatType.normalDamage) { }
    }
}

