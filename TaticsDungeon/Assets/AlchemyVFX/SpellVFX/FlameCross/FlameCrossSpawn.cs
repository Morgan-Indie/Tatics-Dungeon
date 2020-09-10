using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FlameCrossSpawn : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public float lifeTime = 1f;

        public void Initalize(List<GridCell> cells, SkillAbstract skill)
        {
            GameObject ob = Instantiate(projectilePrefab, transform.position, transform.rotation);
            ob.GetComponent<FlameCrossProjectile>().Initalize(cells, skill);
            Destroy(gameObject, lifeTime);
        }
    }
}