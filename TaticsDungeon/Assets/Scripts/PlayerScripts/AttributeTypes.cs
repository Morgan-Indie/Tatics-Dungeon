﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum AttributeType
    {
        strength,
        intelligence,
        vitality,
        dexterity,
        luck,
        stamina,
        level
    }

    public enum EquipmentType
    {
        oneHandedWeapon,
        twoHandedWeapon,
        helmet,
        amulet,
        boots,
        leggings,
        shield,
        torso,
        gloves,
        bow,
        quiver
    }

    public enum DamageStatType
    {
        None,
        normalDamage,
        fireDamage,
        waterDamage,
        shockDamage,
        pierceDamage,
        curseDamage,
        poisonDamage,
    }

    public enum DefenseStatType
    {
        armor,
        fireResistance,
        waterResistance,
        shockResistance,
        poisonResistance,
    }
}
