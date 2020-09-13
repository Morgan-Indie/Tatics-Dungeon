using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class APFill : MonoBehaviour
    {
        [Header("Required")]
        public int APConstriant = 10;

        [HideInInspector]
        public Image[] apPoints;
        public int maxAP;

        public void Awake()
        {
            apPoints = GetComponentsInChildren<Image>();
            for (int i = 0; i < APConstriant; i++)
                apPoints[i].enabled = false;
        }

        public void SetMaxAP(int AP)
        {
            maxAP = AP;            
            for (int i = 0; i < maxAP; i++)
                apPoints[i].enabled = true;
        }

        public void SetCurrentAP(int currentAP)
        {
            for (int i = currentAP; i < maxAP; i++)
                FadeAPOut(apPoints[i]);
        }

        public void RecoverAPUI()
        {
            for (int i = 0; i < maxAP; i++)
                FadeAPIn(apPoints[i]);
        }

        public void FadeAPOut(Image ap)
        {
            LeanTween.alpha(ap.rectTransform, 0, 1f);
        }

        public void FadeAPIn(Image ap)
        {
            LeanTween.alpha(ap.rectTransform, 1, 1f);
        }
    }
}