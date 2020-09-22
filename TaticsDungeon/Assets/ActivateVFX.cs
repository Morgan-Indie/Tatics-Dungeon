using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class ActivateVFX : MonoBehaviour
    {
        public static ActivateVFX Instance = null;
        public GameObject FireVFX;
        public GameObject WetVFX;
        public GameObject OilVFX;
        public GameObject InfernoVFX;
        public GameObject ShockVFX;
        public GameObject PoisonVFX;
        public List<GameObject> BloodVFXs;
        public Vector3 VFXoffset;

        public void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void ActivateElementalEffect(StatusEffect effect, GameObject target)
        {
            switch (effect)
            {
                case StatusEffect.Burning:
                    GameObject Fire = Instantiate(FireVFX) as GameObject;
                    Fire.transform.position = target.transform.position+Vector3.up*.5f;
                    Fire.transform.SetParent(target.transform);
                    break;
                case StatusEffect.Wet:
                    GameObject Wet = Instantiate(WetVFX) as GameObject;
                    Wet.transform.position = target.transform.position + Vector3.up * .5f;
                    Wet.transform.SetParent(target.transform);
                    break;
                case StatusEffect.Oiled:
                    GameObject Oil = Instantiate(OilVFX) as GameObject;
                    Oil.transform.position = target.transform.position + Vector3.up * .5f;
                    Oil.transform.SetParent(target.transform);
                    break;
                case StatusEffect.Shocked:
                    GameObject Shock = Instantiate(ShockVFX) as GameObject;
                    Shock.transform.position = target.transform.position + Vector3.up * .5f;
                    Shock.transform.SetParent(target.transform);
                    break;
                case StatusEffect.Inferno:
                    GameObject Inferno = Instantiate(InfernoVFX) as GameObject;
                    Inferno.transform.position = target.transform.position + Vector3.up * .5f;
                    Inferno.transform.SetParent(target.transform);
                    break;
            }
        }

        public void ActivateHealingEffect(GameObject target)
        {
            GameObject healEffect = Instantiate(HealingVFX) as GameObject;
            healEffect.transform.position = target.transform.position + Vector3.up * .5f;
            healEffect.transform.SetParent(target.transform);
            Destroy(healEffect, 2f);
        }
    }
}
