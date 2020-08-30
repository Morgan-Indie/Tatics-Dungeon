using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class Burn : MonoBehaviour
    {
        public int damage = 10;

        private void OnTriggerEnter(Collider other)
        {
            CharacterStats playerStats = other.GetComponent<CharacterStats>();

            if (playerStats != null)
                playerStats.TakeDamage(damage);
        }
    }
}

