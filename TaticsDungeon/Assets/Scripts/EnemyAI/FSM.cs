using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class FSM : MonoBehaviour
    {
        private List<FSMState> fsmStates;
        public FSMStateID CurrentStateID;
        public FSMState CurrentState;

        public FSM()
        {
            fsmStates = new List<FSMState>();
        }

        // Start is called before the first frame update
        public void AddFSMState(FSMState fsmState)
        {
            if (fsmState == null)
                Debug.LogError("FSM ERROR: Null reference is not allowed");

            if (fsmStates.Count==0)
            {
                fsmStates.Add(fsmState);
                CurrentState = fsmState;
                CurrentStateID = fsmState.ID;
                return;
            }

            foreach(FSMState state in fsmStates)
            {
                if (state.ID == fsmState.ID)
                {
                    Debug.LogError("FSM ERROR: Trying to add a state that was already inside the list");
                    return;
                }
            }

            fsmStates.Add(fsmState);
        }

        public void DeleteState(FSMStateID fsmState)
        {
            if (fsmState == FSMStateID.None)
            {
                Debug.LogError("FSM ERROR: state id is not allowed");
                return;
            }

            foreach(FSMState state in fsmStates)
            {
                if (state.ID == fsmState)
                {
                    fsmStates.Remove(state);
                    return;
                }
            }
            Debug.LogError("FSM ERROR: The state passed was not on the list. Impossible to delete it");
        }

        public void PerformTransition(Transition trans)
        {
            if (trans == Transition.None)
            {
                Debug.LogError("FSM ERROR: Null transition is not allowed");
                return;
            }

            FSMStateID id = CurrentState.GetOutputState(trans);
            if (id==FSMStateID.None)
            {
                Debug.LogError("FSM ERROR: Current State does not have a target state for this transition");
                return;
            }

            CurrentStateID = id;
            foreach (FSMState state in fsmStates)
            {
                if (state.ID==CurrentStateID)
                {
                    CurrentState = state;
                    break;
                }
            }
        }

        private void Start()
        {
            Initialize();
        }

        public virtual void Initialize() { }
        public virtual void FSMUpdate() { }
        public virtual void FSMFixedUpdate(float delta) { }
    }
}

