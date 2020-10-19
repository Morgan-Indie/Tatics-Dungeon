using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class NavDict : MonoBehaviour
    {
        [Header("Required")]
        public CharacterLocation characterLocation;
        public CharacterAP characterAP;
        public bool isDirty = false; 

        Dictionary<IntVector2, IntVector2> _currentNavDict;
        Dictionary<IntVector2, IntVector2> _currentTargetsNavDict;

        public Dictionary<IntVector2, IntVector2> currentNavDict
        {
            get
            {
                if (isDirty)
                {
                    SetCurrentNavDict();
                    isDirty = false;
                }

                return _currentNavDict;
            }
        }

        public Dictionary<IntVector2, IntVector2> currentTargetsNavDict
        {
            get
            {
                if (isDirty)
                {
                    SetCurrentNavDict();
                    isDirty = false;
                }

                return _currentTargetsNavDict;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            characterAP = GetComponent<CharacterAP>();
            characterLocation = GetComponent<CharacterLocation>();
        }
     
        public void SetCurrentNavDict()
        {
            (_currentNavDict, _currentTargetsNavDict) = NavigationHandler.instance.Navigate(
                characterLocation.currentIndex, characterAP.currentAP);
        }      
    }
}

