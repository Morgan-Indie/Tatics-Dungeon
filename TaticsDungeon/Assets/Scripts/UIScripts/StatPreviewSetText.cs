using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class StatPreviewSetText : MonoBehaviour
    {
        public Text[] statTexts;
        public CharacterStats playerStats;
        public Text characterName;

        public void Start()
        {
            playerStats = GetComponentInParent<CharacterStats>();
            statTexts = GetComponentsInChildren<Text>();

            characterName.text = playerStats.characterName;
            updateStatTexts();
        }

        public void updateStatTexts()
        {
            foreach (Text text in statTexts)
            {
                if (text.text.Contains("Luck"))
                    text.text = "Luck  " + playerStats.Luck.Value.ToString();                    
                else if (text.text.Contains("Strength"))
                    text.text = "Strength  " + playerStats.Strength.Value.ToString();
                else if (text.text.Contains("Dexterity"))
                    text.text = "Dexterity  " + playerStats.Dexterity.Value.ToString();
                else if (text.text.Contains("Health"))
                    text.text = "Health  " + playerStats.currentHealth.ToString() + "/" + playerStats.maxHealth.ToString();
                else if (text.text.Contains("Physical"))
                    text.text = "Physical  " + playerStats.normalDamage.Value.ToString();
                else if (text.text.Contains("Armor"))
                    text.text = "Armor  " + playerStats.armor.Value.ToString();
                else if (text.text.Contains("Stamina"))
                    text.text = "Stamina  " + playerStats.Stamina.Value.ToString();
                else if (text.text.Contains("Intelligence"))
                    text.text = "Intelligence  " + playerStats.Intelligence.Value.ToString();
                else if (text.text.Contains("Vitality"))
                    text.text = "Vitality  " + playerStats.Vitality.Value.ToString();
                else if (text.text.Contains("Fire"))
                    text.text = "Fire  " + playerStats.fireDamage.Value.ToString();
                else if (text.text.Contains("Water"))
                    text.text = "Water  " + playerStats.waterDamage.Value.ToString();
                else if (text.text.Contains("Shock"))
                    text.text = "Shock  " + playerStats.shockDamage.Value.ToString();
                else if (text.text.Contains("Pierce"))
                    text.text = "Pierce  " + playerStats.pierceDamage.Value.ToString();
                else if (text.text.Contains("Fire Resistance"))
                    text.text = "Fire Resistance  " + playerStats.fireResistance.Value.ToString();
                else if (text.text.Contains("Water Resistance"))
                    text.text = "Water Resistance  " + playerStats.waterResistance.Value.ToString();
                else if (text.text.Contains("Shock Resistance"))
                    text.text = "Shock Resistance  " + playerStats.shockResistance.Value.ToString();
                else if (text.text.Contains("Poison Resistance"))
                    text.text = "Poison Resistance  " + playerStats.poisonResistance.Value.ToString();
                else if (text.text.Contains("Curse"))
                    text.text = "Curse  " + playerStats.curseDamage.Value.ToString();
            }
        }
    }
}

