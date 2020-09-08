using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCrossSmallProjectile : MonoBehaviour
    {
        public GameObject smallImpactPrefab;
        public float projectileSpeed = 1.5f;
        public LeanTweenType projectileTween;
        public Vector3 impactOffset;
        GridCell destination;
        SkillAbstract skill;

        public void Initalize(GridCell cell, SkillAbstract _skill)
        {
            destination = cell;
            skill = _skill;
            LeanTween.move(gameObject, destination.transform.position + impactOffset, 1f / projectileSpeed).setOnComplete(TravelComplete).setEase(projectileTween);
        }

        void TravelComplete()
        {
            GameObject ob = Instantiate(smallImpactPrefab, transform.position, Quaternion.identity);
            ob.GetComponent<FlameCrossSmallImpact>().Initalize(destination, skill);
            Destroy(gameObject);
        }
    }
}