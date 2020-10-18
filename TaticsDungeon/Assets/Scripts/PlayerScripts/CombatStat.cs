using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class DamageStat : Stat
    {
        public DamageStatType type;

        public DamageStat(float baseValue, DamageStatType StatType) : base(baseValue)
        {
            type = StatType;
        }
    }

    public class DefenseStat : Stat
    {
        public DefenseStatType type;

        public DefenseStat(float baseValue, DefenseStatType StatType) : base(baseValue)
        {
            type = StatType;
        }
    }
}

