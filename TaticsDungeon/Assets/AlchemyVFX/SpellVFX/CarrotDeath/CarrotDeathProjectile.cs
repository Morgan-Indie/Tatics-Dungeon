using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CarrotDeathProjectile : MonoBehaviour
    {
        GridCell destination;
        public GameObject impactPrefab;
        public float fixedUpOffset = 1f;
        public float offsetRadius = 1f;
        public float offsetDecrease = 0.3f;
        public float projectileSpeed = 1f;
        public Vector3 carrotSize;
        Vector3 offsetVector;
        public LeanTweenType projectileTween;
        SkillAbstract skill;
        Vector3 offsetDestination;
        Vector3 origin;
        bool excute;

        public void Initalize(GridCell cell, SkillAbstract _skill, bool _excute)
        {
            destination = cell;
            skill = _skill;
            excute = _excute;
            offsetVector = new Vector3(
                    Random.Range(-offsetRadius, offsetRadius),
                    Random.Range(-offsetRadius, offsetRadius) + fixedUpOffset,
                    Random.Range(-offsetRadius, offsetRadius)
                );
            transform.position += offsetVector;
            origin = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            offsetDestination = cell.transform.position + offsetVector * offsetDecrease;
            transform.LookAt(offsetDestination);
            transform.localScale = Vector3.zero;
            LeanTween.scale(gameObject, carrotSize, Random.Range(0.5f, 1.5f)).setOnComplete(GrowComplete);

        }

        public void GrowComplete()
        {
            float speedModifier = Random.Range(0.8f, 1.2f);
            LeanTween.move(gameObject, offsetDestination, (1f * speedModifier) / projectileSpeed).setOnComplete(TravelComplete).setEase(projectileTween);
        }

        public void TravelComplete()
        {
            GameObject ob = Instantiate(impactPrefab, transform.position, transform.rotation);
            ob.transform.LookAt(origin);
            ob.GetComponent<CarrotDeathImpact>().Initalize(destination, skill, excute);
            Destroy(gameObject);
        }
    }
}