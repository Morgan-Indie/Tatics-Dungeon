using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCrossProjectile : MonoBehaviour
    {
        public GameObject imapctPrefab;
        public float projectileSpeed = 0.5f;
        public Vector3 impactOffset;
        public LeanTweenType projectileEase;
        List<GridCell> cells;
        SkillAbstract skill;

        public void Initalize(List<GridCell> _cells, SkillAbstract _skill)
        {
            cells = _cells;
            skill = _skill;
            LeanTween.move(gameObject, cells[0].transform.position + impactOffset, 1f / projectileSpeed).setOnComplete(TravelComplete).setEase(projectileEase);
            transform.LookAt(cells[0].transform);
        }

        void TravelComplete()
        {
            GameObject ob = Instantiate(imapctPrefab, transform.position, Quaternion.identity);
            ob.GetComponent<FlameCrossImpact>().Initalize(cells, skill);
            Destroy(gameObject);
        }
    }
}