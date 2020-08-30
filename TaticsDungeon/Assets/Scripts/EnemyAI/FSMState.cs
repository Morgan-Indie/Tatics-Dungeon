using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum Transition
    {
        None,
        OutNumberedByPlayer,
        SupportedByAgents,
        PlayerLowHealth,
        AgentLowHealth,
        AgentDead
    }

    public enum FSMStateID
    {
        None,
        Aggressive,
        Defensive,
        Evasive,
        Opportunistic,
        Dead,
        Coopertive
    }

    public abstract class FSMState
    {
        protected Dictionary<Transition, FSMStateID> map = new Dictionary<Transition, FSMStateID>();
        protected FSMStateID stateID;
        public FSMStateID ID { get { return stateID; } }
        protected Vector3 destPos;
        protected Transform[] waypoints;
        
        public void AddTransition(Transition transition, FSMStateID id)
        {
            if (transition == Transition.None||id == FSMStateID.None)
            {
                Debug.LogWarning("FSMState : Null transition not allowed");
                return;
            }

            if (map.ContainsKey(transition))
            {
                Debug.LogWarning("FSMState ERROR: transition is already inside the map");
                return;
            }

            map.Add(transition, id);
            Debug.Log("Added : " + transition + " with ID : " + id);
        }

        public FSMStateID GetOutputState(Transition trans)
        {
            if(trans==Transition.None)
            {
                Debug.LogError("FSMState ERROR: NullTransition is not allowed");
                return FSMStateID.None;
            }

            if(map.ContainsKey(trans))
            {
                return map[trans];
            }

            Debug.LogError("FSMState ERROR: " + trans + "Transition passed to the State was not on the list");
            return FSMStateID.None;
        }

        public abstract void HandleTransitions();
        public abstract void Act(float delta);
    }
}

