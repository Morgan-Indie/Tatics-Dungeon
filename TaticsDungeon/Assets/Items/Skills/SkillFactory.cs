using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace PrototypeGame
{
    public enum SkillType
    {
        MeleeAttack,
        Move,
        ShieldCharge,
        RangeAttack,
        Castable,
    }

    [System.Serializable]
    public class SkillFactory:MonoBehaviour
    {
        public static SkillFactory instance = null;

        public void Awake()
        {
            if (instance==null)
                instance = this;
        }

        public void Activate(CharacterStats characterStats,AnimationHandler animationHandler,TaticalMovement taticalMovement, Skill skill,float delta)
        {
            switch (skill.type)
            {
                case SkillType.MeleeAttack:
                    MeleeAttack.Activate(characterStats,animationHandler, taticalMovement, skill, delta);
                    break;
                case SkillType.Move:
                    taticalMovement.ExcuteMovement(delta);
                    break;
                case SkillType.ShieldCharge:
                    ShieldCharge.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                    break;
                case SkillType.RangeAttack:
                    RangeAttack.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                    break;
                case SkillType.Castable:
                    switch (skill.castableSettings.castableSpell)
                    {
                        case (CastableSpell.CastFire):
                            CastFire.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                            break;
                        case (CastableSpell.CastChill):
                            CastChill.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                            break;
                        case (CastableSpell.CastOil):
                            CastOil.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                            break;
                        case (CastableSpell.CastWater):
                            CastWater.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                            break;
                        case (CastableSpell.CastPoison):
                            CastPoison.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                            break;
                        case (CastableSpell.CastShock):
                            CastShock.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                            break;
                        case (CastableSpell.FireStorm):
                            FireStorm.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                            break;
                        case (CastableSpell.IceBomb):
                            IceBomb.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                            break;
                        case (CastableSpell.EnergyBall):
                            EnergyBall.Activate(characterStats, animationHandler, taticalMovement, skill, delta);
                            break;
                    }
                    break;
            }
        }
    }
}
