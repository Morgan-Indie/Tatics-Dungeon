using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class EndTurn : UIButtonScript
    {
        public Image buttonImage;
        public Button endTurnButton;

        // Start is called before the first frame update
        void Start()
        {
            endTurnButton = GetComponent<Button>();
            buttonImage = GetComponent<Image>();
        }

        public void EndTurnClick()
        {
            GameManager.instance.SwitchTurns();
        }
    }
}
