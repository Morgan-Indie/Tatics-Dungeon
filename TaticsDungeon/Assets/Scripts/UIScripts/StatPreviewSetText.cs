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
        }

        public void updateStatTexts()
        {
            foreach (Text text in statTexts)
            {
                if (text.text=="Luck")
                    text.text = "Luck  " + playerStats.Luck.Value.ToString();                    
                else if (text.text==("Strength"))
                    text.text = "Strength  " + playerStats.Strength.Value.ToString();
                else if (text.text==("Dexterity"))
                    text.text = "Dexterity  " + playerStats.Dexterity.Value.ToString();
                else if (text.text==("Health"))
                    text.text = "Health  " + playerStats.currentHealth.ToString() + "/" + playerStats.maxHealth.ToString();
                else if (text.text==("Physical"))
                    text.text = "Physical  " + playerStats.normalDamage.Value.ToString();
                else if (text.text==("Armor"))
                    text.text = "Armor  " + playerStats.armor.Value.ToString();
                else if (text.text==("Stamina"))
                    text.text = "Stamina  " + playerStats.Stamina.Value.ToString();
                else if (text.text==("Intelligence"))
                    text.text = "Intelligence  " + playerStats.Intelligence.Value.ToString();
                else if (text.text==("Vitality"))
                    text.text = "Vitality  " + playerStats.Vitality.Value.ToString();
                else if (text.text==("Fire"))
                    text.text = "Fire  " + playerStats.fireDamage.Value.ToString();
                else if (text.text==("Water"))
                    text.text = "Water  " + playerStats.waterDamage.Value.ToString();
                else if (text.text==("Shock"))
                    text.text = "Shock  " + playerStats.shockDamage.Value.ToString();
                else if (text.text==("Pierce"))
                    text.text = "Pierce  " + playerStats.pierceDamage.Value.ToString();
                else if (text.text==("Fire Resistance"))
                    text.text = "Fire Resistance  " + playerStats.fireResistance.Value.ToString();
                else if (text.text==("Water Resistance"))
                    text.text = "Water Resistance  " + playerStats.waterResistance.Value.ToString();
                else if (text.text==("Shock Resistance"))
                    text.text = "Shock Resistance  " + playerStats.shockResistance.Value.ToString();
                else if (text.text==("Poison Resistance"))
                    text.text = "Poison Resistance  " + playerStats.poisonResistance.Value.ToString();
                else if (text.text==("Curse"))
                    text.text = "Curse  " + playerStats.curseDamage.Value.ToString();
            }
        }
    }
}

