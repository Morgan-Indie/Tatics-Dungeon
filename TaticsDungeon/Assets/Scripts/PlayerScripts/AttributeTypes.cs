using System.Collections;
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
        leggins,
        shield,
        torso,
        gloves,
        bow,
        quiver
    }


    public enum CombatStatType
    {
        normalDamage,
        fireDamage,
        waterDamage,
        shockDamage,
        pierceDamage,
        curseDamage,
        poisonDamage,
        armor,
        fireResistance,
        waterResistance,
        shockResistance,
        poisonResistance,
    }
}
