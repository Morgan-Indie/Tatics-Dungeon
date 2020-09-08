using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class InstantCastProjectile : MonoBehaviour
    {
        GridCell destination;
        public GameObject impactPrefab;
        public float offset = 1f;
        public float projectileSpeed = 1f;
        public LeanTweenType projectileTween;
        SkillAbstract skill;

        public void Initalize(GridCell cell, SkillAbstract _skill)
        {
            destination = cell;
            skill = _skill;
            Vector3 offsetPos = new Vector3(cell.transform.position.x, cell.transform.position.y + offset, cell.transform.position.z);
            transform.LookAt(offsetPos);
            LeanTween.move(gameObject, offsetPos, 1f / projectileSpeed).setOnComplete(TravelComplete).setEase(projectileTween);
        }

        public void TravelComplete()
        {
            GameObject ob = Instantiate(impactPrefab, transform.position, transform.rotation);
            ob.GetComponent<InstantCastImpact>().Initalize(destination, skill);
            Destroy(gameObject);
        }
    }
}
