using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public class CellAlchemyState : MonoBehaviour
    {
        public GasPhaseState gasState = GasPhaseState.Dry;
        public LiquidPhaseState liquidState = LiquidPhaseState.Dry;
        public SolidPhaseState solidState = SolidPhaseState.Dry;
        public BlessingState blessingState = BlessingState.Neutral;
        public FireState fireState = FireState.Dry;
        public ShockState shockState = ShockState.Dry;

        public GameObject gasEffect;
        public GameObject liquidEffect;
        public GameObject solidEffect;
        public GameObject blessedEffect;
        public GameObject fireEffect;
        public GameObject shockEffect;

        public int fireTurnsRemaining = 0;
        public int shockTurnsRemaining = 0;
        public int poisonTurnsRemaining = 0;
        public int frozenTurnsRemaining = 0;

        public List<AlchemyChangeType> changedValues;

        private void Awake()
        {
            changedValues = new List<AlchemyChangeType>();
        }

        public void CopyInformation(CellAlchemyState copy)
        {
            gasState = copy.gasState;
            liquidState = copy.liquidState;
            solidState = copy.solidState;
            blessingState = copy.blessingState;
            fireState = copy.fireState;
            shockState = copy.shockState;
            changedValues.Clear();
            foreach(AlchemyChangeType change in copy.changedValues) { changedValues.Add(change); }
        }

        public List<AlchemyChangeType> GetChangedValues()
        {
            List<AlchemyChangeType> result = new List<AlchemyChangeType>();
            foreach (AlchemyChangeType change in changedValues)
            {
                if (!result.Contains(change)) { result.Add(change); }
            }
            changedValues = result;
            return changedValues;
        }

        public void UpdateTurn()
        {
            if (fireTurnsRemaining > 0)
            {
                fireTurnsRemaining--;
                if (fireTurnsRemaining <= 0)
                    Destroy(fireEffect);
            }
            if (shockTurnsRemaining > 0)
            {
                shockTurnsRemaining--;
                if (shockTurnsRemaining <= 0)
                    Destroy(shockEffect);
            }

        }
    }
}