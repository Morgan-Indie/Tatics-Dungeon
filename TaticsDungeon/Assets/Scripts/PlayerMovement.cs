using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  PrototypeGame
{
    public class PlayerMovement : MonoBehaviour
    {
        public IntVector2 prevIndex;
        public NavDict navDict;
        public CharacterAP ap;
        public TacticalMovement tacticalMovement;
        public CharacterStateManager stateManager;

        // Start is called before the first frame update
        void Start()
        {
            navDict = GetComponent<NavDict>();
            ap = GetComponent<CharacterAP>();
            tacticalMovement = GetComponent<TacticalMovement>();
            stateManager = GetComponent<CharacterStateManager>();
        }

        public bool checkAP(IntVector2 index)
        {
            if (ap.currentAP >= MovementUtils.Instance.GetRequiredMoves(index, tacticalMovement.path))
                return true;
            return false;
        }

        public void ExcuteMovement(float delta)
        {
            if (stateManager.characterAction == CharacterAction.Moving)
                tacticalMovement.TraverseToDestination(delta);

            else
            {
                IntVector2 mouseIndex = MovementUtils.Instance.GetMouseIndex();
                if(navDict.currentNavDict.ContainsKey(mouseIndex) && checkAP(mouseIndex))
                {
                    if (!mouseIndex.Equals(prevIndex))
                    {
                        tacticalMovement.SetPath();
                        GridManager.Instance.HighlightPathWithList(tacticalMovement.path);
                    }

                    if (Input.GetMouseButtonDown(0) && stateManager.characterAction == CharacterAction.None)
                    {
                        tacticalMovement.SetTargetDestination(mouseIndex);
                        stateManager.characterAction = CharacterAction.Moving;
                    }
                }
            }
        }
    }
}
