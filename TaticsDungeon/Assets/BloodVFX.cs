using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class BloodVFX : MonoBehaviour
    {
        public List<GameObject> bloodEffectsSplash;
        public List<GameObject> bloodEffectsPerice;

        public void PlaySplashBloodEffects()
        {
            int choice = Random.Range(0, bloodEffectsSplash.Count);
            bloodEffectsSplash[choice].GetComponent<ParticleSystem>().Play();
        }

        public void PlayPeirceBloodEffects()
        {
            int choice = Random.Range(0, bloodEffectsPerice.Count);
            bloodEffectsPerice[choice].GetComponent<ParticleSystem>().Play();
        }
    }
}
