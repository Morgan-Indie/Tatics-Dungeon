using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class ArrowHolder : MonoBehaviour
    {
        public CharacterStats characterStats;
        public SkillAbstract rangeAttack;
        public GameObject arrowPrefab;
        public GameObject arrow;
        public GameObject target;
        public GridCell targetCell;
        public Transform parentOverride;

        public void Start()
        {
            characterStats = GetComponent<CharacterStats>();
        }

        public void Draw()
        {
            arrow = Instantiate(arrowPrefab);
            arrow.transform.position = parentOverride.position;
            arrow.transform.SetParent(parentOverride);
        }

        public void Release()
        {
            if (arrow != null)
            {
                arrow.transform.SetParent(null);
                arrow.transform.LookAt(target.transform);
                arrow.transform.Rotate(Vector3.up*45f+ Vector3.right * 15f);

                LeanTween.move(arrow, target.transform.position, .2f).setOnComplete(ArrowHit);
            }

            else
                Debug.LogError("Arrow not instantiated");
        }

        public void ArrowHit()
        {
            rangeAttack.Excute(Time.deltaTime, targetCell);
            Destroy(arrow);
        }
    }
}

