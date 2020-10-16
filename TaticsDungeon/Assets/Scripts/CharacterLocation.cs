using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CharacterLocation : MonoBehaviour
    {
        [Header("Required")]
        public GridMapAdapter mapAdapter;

        public GridCell currentCell
        {
            get { return mapAdapter.GetCellByPos(transform.position); }
        }

        public IntVector2 currentIndex
        {
            get{ return currentCell.index; }
        }

        // Start is called before the first frame update
        void Start()
        {
            mapAdapter = GridManager.Instance.mapAdapter;
        }
    }
}

