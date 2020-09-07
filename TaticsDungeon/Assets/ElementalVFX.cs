using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class ElementalVFX : MonoBehaviour
    {
        public static ElementalVFX Instance = null;
        public GameObject FireVFX;
        public GameObject WetVFX;
        public GameObject OilVFX;
        public GameObject InfernoVFX;
        public GameObject ShockVFX;
        public GameObject PoisonVFX;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void ActivateEffect(StatusEffect effect, GameObject target)
        {

            switch (effect)
            {
                case StatusEffect.Burning:
                    GameObject Fire = Instantiate(FireVFX) as GameObject;
                    Fire.transform.position = target.transform.position;
                    Fire.transform.SetParent(target.transform);
                    break;
                case StatusEffect.Wet:
                    GameObject Wet = Instantiate(WetVFX) as GameObject;
                    Wet.transform.position = target.transform.position;
                    Wet.transform.SetParent(target.transform);
                    break;
                case StatusEffect.Oiled:
                    GameObject Oil = Instantiate(OilVFX) as GameObject;
                    Oil.transform.position = target.transform.position;
                    Oil.transform.SetParent(target.transform);
                    break;
            }
        }

    }
}

