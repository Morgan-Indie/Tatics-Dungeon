using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CombatUtils : MonoBehaviour
    {
        [Header("Not Required")]
        public CharacterStats characterStats;
        public EquipmentSlotManager equipmentSlotManager;

        public void Start()
        {
            characterStats = GetComponent<CharacterStats>();
        }

        public DamageStruct ComputeDamage()
        {
            float totalNormalDamage = 0f;
            float totalPierceDamage = 0f;
            float totalElementalDamge = 0f;

            foreach (var damageStat in characterStats.playerCombatStatDict)
                if (damageStat.Key == CombatStatType.normalDamage)
                    totalNormalDamage += damageStat.Value.Value;
                else if (damageStat.Key == CombatStatType.pierceDamage)
                    totalPierceDamage += damageStat.Value.Value;
                else if (damageStat.Key == CombatStatType.armor || damageStat.Key == CombatStatType.resistance)
                    totalPierceDamage += 0;
                else
                    totalElementalDamge += damageStat.Value.Value;

            DamageStruct outputDamage = new DamageStruct(totalElementalDamge, totalPierceDamage, totalNormalDamage);
            return outputDamage;
        }

        public void Attack(GameObject targetCharacter)
        {
            CharacterStats targetStats = targetCharacter.GetComponent<CharacterStats>();
            DamageStruct outputDamage = ComputeDamage();
            int damageDeltNormal = outputDamage.normal -(int)targetStats.armor.Value;
            int damageDeltPierce = outputDamage.pierce;
            int damageDeltElemental = outputDamage.elemental-(int)targetStats.resistance.Value;
            
            int totalDamage = damageDeltPierce + damageDeltElemental + damageDeltNormal;
            targetStats.TakeDamage(totalDamage);
        }
    }
}

