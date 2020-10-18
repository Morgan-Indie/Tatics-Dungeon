using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CharacterAP : MonoBehaviour
    {
        public int maxAP;
        public int currentAP;
        public CharacterStats characterStats;
        public GameObject statusPanel;
        public APFill apBar;
        public PlayerManager playerManager;

        // Start is called before the first frame update
        void Start()
        {
            characterStats = GetComponent<CharacterStats>();
            apBar = statusPanel.GetComponentInChildren<APFill>();
            playerManager = GetComponent<PlayerManager>();

            SetMaxAPFromStamina();
            currentAP = maxAP;
            apBar.SetMaxAP(maxAP);
        }

        public void SetMaxAPFromStamina()
        {
            maxAP = Mathf.FloorToInt(characterStats.Stamina.Value);
        }

        public void UseAP(int AP)
        {
            currentAP -= AP;
            currentAP = currentAP >= 0 ? currentAP : 0;
            apBar.SetCurrentAP(currentAP);
        }

        public void Recover(int AP = 4)
        {
            currentAP += AP;
            currentAP = currentAP <= maxAP ? currentAP : maxAP;
            apBar.SetCurrentAP(currentAP);
        }
    }
}
