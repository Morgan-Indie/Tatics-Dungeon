using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CombatStat : Stat
    {
        public CombatStatType type;

        public CombatStat(float baseValue, CombatStatType StatType) : base(baseValue)
        {
            type = StatType;
        }
    }
}

