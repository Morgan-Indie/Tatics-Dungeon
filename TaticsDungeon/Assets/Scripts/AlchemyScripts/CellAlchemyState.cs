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

        public GameObject gasEffect;
        public GameObject liquidEffect;
        public GameObject solidEffect;
        public GameObject blessedEffect;
        public GameObject fireEffect;
    }
}