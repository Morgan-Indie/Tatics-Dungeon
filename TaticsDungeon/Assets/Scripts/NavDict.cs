using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class NavDict : MonoBehaviour
    {
        public CharacterAP characterAP;
        public CharacterLocation location;
        public bool isDirty;

        public Dictionary<IntVector2, IntVector2> _currentNavDict;
        public Dictionary<IntVector2, IntVector2> _currentTargetsNavDict;

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

        public void Start()
        {
            characterAP = GetComponent<CharacterAP>();
        }

        public void SetCurrentNavDict()
        {
            (_currentNavDict, _currentTargetsNavDict) = NavigationHandler.instance.Navigate(location.currentIndex, characterAP.currentAP);
        }
    }
}

