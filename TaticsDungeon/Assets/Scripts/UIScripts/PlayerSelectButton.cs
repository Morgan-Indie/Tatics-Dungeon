using System;
using UnityEngine.EventSystems;
using TMPro;

namespace PrototypeGame
{
    public class PlayerSelectButton : UIButtonScript
    {        
        public void NextPlayer()
        {
            GameManager.instance.SetNextPlayer();
        }
        public void PreviousPlayer()
        {
            GameManager.instance.SetPreviousPlayer();
        }
    }
}

