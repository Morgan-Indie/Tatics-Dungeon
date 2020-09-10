using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CarrotDeathSpawn : MonoBehaviour
    {
        public GameObject projectilePrefab;
        public float lifeTime = 1.5f;
        public float lowerSpawn = 10;
        public float upperSpawn = 15;

        public void Initalize(List<GridCell> cells, SkillAbstract skill)
        {
            foreach (GridCell cell in cells)
            {
                int iters = Mathf.FloorToInt(Random.Range(lowerSpawn, upperSpawn));
                for (int i = 0; i < iters; i++)
                {
                    transform.LookAt(cell.transform);
                    GameObject ob = Instantiate(projectilePrefab, transform.position - Vector3.up * transform.position.y + Vector3.forward * 0.5f, Quaternion.identity);
                    ob.GetComponent<CarrotDeathProjectile>().Initalize(cell, skill, i == 0 ? true : false);
                    Destroy(gameObject, lifeTime);
                }
            }
        }
    }
}