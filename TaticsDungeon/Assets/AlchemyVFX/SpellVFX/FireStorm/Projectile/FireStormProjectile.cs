using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FireStormProjectile : MonoBehaviour
    {
        public LeanTweenType travelType;
        public GridCell destination;
        public float projectileSpeed = 1;
        public float upOffset = 0.5f;
        public GameObject ImpactPrefab;

        public void Initalize(GridCell d)
        {
            destination = d;
            transform.LookAt(d.transform);
            LeanTween.move(gameObject, new Vector3(d.transform.position.x, d.transform.position.y, d.transform.position.z), 1f / projectileSpeed).setOnComplete(TravelComplete).setEase(travelType);
        }

        void TravelComplete()
        {
            GameObject ob = Instantiate(ImpactPrefab, destination.transform.position, destination.transform.rotation);
            ob.GetComponent<FireStormImpact>().Initalize(destination);
            Destroy(gameObject);
        }
    }
}
