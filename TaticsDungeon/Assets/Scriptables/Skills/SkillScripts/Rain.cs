using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class Rain : CastSubstance
    {
        public override SkillAbstract AttachSkill(CharacterManager _character, Skill _skill, SkillSlot _slot)
        {
            Rain rain = _character.gameObject.AddComponent<Rain>();
            rain.character = _character;
            rain.skill = _skill;
            rain.skillAnimation = "SpellCastHand";
            rain.slot = _slot;

            rain.heatState = new HeatState(HeatValue.neutral);
            rain._substance = new AlchemicalSubstance(AlchemicalState.liquid);            
        
            rain.damageStatsList = new List<DamageStat>();
            return rain;
        }
    }
}