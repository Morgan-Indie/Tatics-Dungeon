using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class ShowModel : MonoBehaviour
    {
        public Transform characterModel;
        // Start is called before the first frame update
        void Awake()
        {
            characterModel.gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}

