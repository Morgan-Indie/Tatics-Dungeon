using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CharacterAP : MonoBehaviour
    {
        public int maxAP;
        public int currentAP;
        public APFill apBar;
        public GameObject statusPanel;
        public CharacterStats characterStats;

        // Start is called before the first frame update
        void Start()
        {
            apBar = statusPanel.GetComponentInChildren<APFill>();
            characterStats = GetComponent<CharacterStats>();
            SetMaxAPFromStamina();
            currentAP = maxAP;
        }

        public void SetMaxAPFromStamina()
        {
            maxAP = Mathf.FloorToInt(characterStats.Stamina.Value);
        }

        public void UseAP(int AP)
        {
            currentAP -= AP;
            apBar.SetCurrentAP(currentAP);
        }
    }
}

