using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class CharacterStatusLayout : MonoBehaviour
    {
        [Header("Required")]
        public VerticalLayoutGroup playerLayout;
        public VerticalLayoutGroup enemyLayout;

        public void AddPlayerStatusPanel(PlayerManager player)
        {
            player.characterStats.statusPanel.transform.SetParent(playerLayout.transform,false);
        }

        public void AddEnemyStatusPanel(EnemyManager enemy)
        {
            enemy.characterStats.statusPanel.transform.SetParent(enemyLayout.transform,false);
        }
    }
}

