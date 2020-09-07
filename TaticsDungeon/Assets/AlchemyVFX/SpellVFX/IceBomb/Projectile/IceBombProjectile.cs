using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class IceBombProjectile : MonoBehaviour
    {
        GridCell destination;
        public GameObject impactEffectPrefab;
        public float projectileSpeed = 2f;
        public float arcHeight = 3f;
        Vector3 lastPos;
        public LeanTweenType upEase;
        public LeanTweenType downEase;

        public void Initalize(GridCell d)
        {
            lastPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            destination = d;
            LeanTween.move(gameObject, Vector3.Lerp(d.transform.position, transform.position, 0.5f) + new Vector3(0, arcHeight, 0), 0.5f / projectileSpeed).setOnComplete(StartFall).setEase(upEase);
        }

        private void Update()
        {
            Vector3 aheadPos = transform.position - lastPos;
            if (aheadPos.magnitude >= 0.01f)
            {
                transform.LookAt(transform.position + aheadPos);
                lastPos = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
        }

        void StartFall()
        {
            LeanTween.move(gameObject, destination.transform.position, 0.5f / projectileSpeed).setOnComplete(TravelComplete).setEase(downEase);
        }

        void TravelComplete()
        {
            GameObject ob = Instantiate(impactEffectPrefab, destination.transform.position, destination.transform.rotation);
            ob.GetComponent<IceBombImpact>().Initalize(destination);
            Destroy(gameObject);
        }
    }
}