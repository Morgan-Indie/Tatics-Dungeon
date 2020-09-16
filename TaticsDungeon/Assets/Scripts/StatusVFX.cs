using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class StatusVFX : MonoBehaviour
    {
        public GameObject StunnedVFX;

        public void StunVFXPlay()
        {
            StunnedVFX.GetComponent<ParticleSystem>().Play();
        }

        public void StunVFXStop()
        {
            StunnedVFX.GetComponent<ParticleSystem>().Stop();
        }
    }
}

