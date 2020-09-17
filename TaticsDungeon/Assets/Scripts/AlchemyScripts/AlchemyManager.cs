using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PrototypeGame
{
    public enum GasPhaseState { Dry, Water, Poison };
    public enum LiquidPhaseState { Dry, Water, Poison, Oil };
    public enum SolidPhaseState { Dry, Water, Poison };

    public enum BlessingState { Neutral, Blessed, Cursed};

    public enum FireState { Chill, Dry, Burning, Inferno};

    public enum ShockState { Dry, Shocked};

    public enum AlchemyChangeType { Heat, Solid, Liquid, Gas, Bless, Shock }

    public class AlchemyManager : MonoBehaviour
    {
        
     //   enum ChangeValues { Gas, Liquid, Solid, Blessed, Fire, Shock};
      //  List<ChangeValues> changeValues;

        public static AlchemyManager Instance { get; private set; }

        [Header("Water")]
        public GameObject WaterGasEffectPrefab;
        public GameObject WaterLiquidEffectPrefab;
        public GameObject WaterSolidEffectPrefab;

        [Header("Poison")]
        public GameObject PoisonGasEffectPrefab;
        public GameObject PoisonLiquidEffectPrefab;
        public GameObject PoisonSolidEffectPrefab;

        [Header("Oil")]
        public GameObject OilLiquidEffectPrefab;

        [Header("Fire")]
        public GameObject BurningEffectPrefab;
        public GameObject InfernoEffectPrefab;

        [Header("Chill")]
        public GameObject ChillEffectPrefab;

        [Header("Shock")]
        public GameObject ShockSolidEffectPrefab;
        public GameObject ShockLiquidEffectPrefab;
        public GameObject ShockGasEffectPrefab;

        [Header("Player Status Times")]
        public int playerBurnTime = 2;
        public int playerInfernoTime = 2;
        public int playerPoisonTime = 4;
        public int playerShockTime = 2;
        public int playerFrozenTime = 2;
        public int playerCurseTime = 10;
        public int playerBlessTime = 10;
        public int playerWetTime = 4;
        public int playerOilTime = 4;
        public int playerElectricuteTime = 2;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
        }

        public void ApplyGas(CellAlchemyState cellState, GasPhaseState gas)
        {
            ApplyGasInternal(cellState, gas);
            ApplyVFX(cellState);
        }
        void ApplyGasInternal(CellAlchemyState cellState, GasPhaseState gas)
        {
            cellState.gasState = gas;
            cellState.changedValues.Add(AlchemyChangeType.Gas);
            ShockCheck(cellState);
        }
        public CellAlchemyState SimulateGas(CellAlchemyState cellState, GasPhaseState gas)
        {
            CellAlchemyState simulateState = new CellAlchemyState();
            simulateState.CopyInformation(cellState);
            ApplyGasInternal(simulateState, gas);
            return simulateState;
        }

        public void ApplyLiquid(CellAlchemyState cellState, LiquidPhaseState liquid)
        {
            ApplyLiquidInternal(cellState, liquid);
            ApplyVFX(cellState);
        }
        void ApplyLiquidInternal(CellAlchemyState cellState, LiquidPhaseState liquid)
        {
            cellState.liquidState = liquid;
            cellState.changedValues.Add(AlchemyChangeType.Liquid);
            if ((int)cellState.fireState > (int)FireState.Dry)
            {
                ApplyHeatInternal(cellState);
                if (liquid != LiquidPhaseState.Oil)
                    ReduceFireState(cellState);
            }
            else if ((int)cellState.fireState < (int)FireState.Dry)
            {
                ApplyChillInternal(cellState);
            }
            ShockCheck(cellState);
        }
        public CellAlchemyState SimulateLiquid(CellAlchemyState cellState, LiquidPhaseState liquid)
        {
            CellAlchemyState simulateState = new CellAlchemyState();
            simulateState.CopyInformation(cellState);
            ApplyLiquidInternal(simulateState, liquid);
            return simulateState;
        }

        public void ApplySolid(CellAlchemyState cellState, SolidPhaseState solid)
        {
            ApplySolidInternal(cellState, solid);
            ApplyVFX(cellState);
        }
        void ApplySolidInternal(CellAlchemyState cellState, SolidPhaseState solid)
        {
            cellState.solidState = solid;
            cellState.changedValues.Add(AlchemyChangeType.Solid);
            if ((int)cellState.fireState > (int)FireState.Dry)
            {
                ApplyHeatInternal(cellState, 2);
            }
            ShockCheck(cellState);
        }
        public CellAlchemyState SimulateSolid(CellAlchemyState cellState, SolidPhaseState solid)
        {
            CellAlchemyState simulateState = new CellAlchemyState();
            simulateState.CopyInformation(cellState);
            ApplySolidInternal(simulateState, solid);
            return simulateState;
        }

        public void ApplyHeat(GridCell cell)
        {
            CellAlchemyState cellState = cell.alchemyState;
            ApplyHeatInternal(cellState);
            ApplyVFX(cellState);
            if (cell.occupyingObject != null)
                cell.occupyingObject.GetComponent<CharacterStateManager>();
        }
        void ApplyHeatInternal(CellAlchemyState cellState, int itters = 1)
        {
            LiquidToGas(cellState);
            SolidToLiquid(cellState);
            if (itters - 1 > 0)
                ApplyHeatInternal(cellState, itters - 1);
        }
        public CellAlchemyState SimulateHeat(CellAlchemyState cellState)
        {
            CellAlchemyState simulateState = new CellAlchemyState();
            simulateState.CopyInformation(cellState);
            ApplyHeatInternal(simulateState);
            return simulateState;
        }

        public void ApplyChill(GridCell cell)
        {
            CellAlchemyState cellState = cell.alchemyState;
            ApplyChillInternal(cellState);
            ApplyVFX(cellState);
            if (cell.occupyingObject != null)
                cell.occupyingObject.GetComponent<CharacterStateManager>();
        }
        void ApplyChillInternal(CellAlchemyState cellState, int itters = 1)
        {
            LiquidToSolid(cellState);
            GasToLiquid(cellState);
            ReduceFireWithChill(cellState);
            if (itters - 1 > 0)
                ApplyChillInternal(cellState, itters - 1);
            else
                if (cellState.changedValues.Contains(AlchemyChangeType.Solid) || cellState.changedValues.Contains(AlchemyChangeType.Liquid) && cellState.fireState == FireState.Chill)
                    cellState.fireState = FireState.Dry;
        }
        public CellAlchemyState SimulateChill(CellAlchemyState cellState)
        {
            CellAlchemyState simulateState = new CellAlchemyState();
            simulateState.CopyInformation(cellState);
            ApplyChillInternal(simulateState);
            return simulateState;
        }

        public void ApplyShock(GridCell cell)
        {
            CellAlchemyState cellState = cell.alchemyState;
            ApplyShockInternal(cellState);
            ApplyVFX(cellState);
            if (cell.occupyingObject != null)
                cell.occupyingObject.GetComponent<CharacterStateManager>();
        }
        void ApplyShockInternal(CellAlchemyState cellState)
        {
            cellState.shockState = ShockState.Shocked;
            cellState.changedValues.Add(AlchemyChangeType.Shock);
        }
        public CellAlchemyState SimulateShock(CellAlchemyState cellState)
        {
            CellAlchemyState simulateState = new CellAlchemyState();
            simulateState.CopyInformation(cellState);
            ApplyShockInternal(simulateState);
            return simulateState;
        }

        void SolidToLiquid(CellAlchemyState cellState)
        {
            if (cellState.solidState == SolidPhaseState.Dry)
                return;
            cellState.liquidState = (LiquidPhaseState)cellState.solidState;
            cellState.changedValues.Add(AlchemyChangeType.Liquid);
            ReduceFireState(cellState);
            cellState.solidState = SolidPhaseState.Dry;
            cellState.changedValues.Add(AlchemyChangeType.Solid);
            ShockCheck(cellState);
        }
        void LiquidToGas(CellAlchemyState cellState)
        {
            switch (cellState.liquidState)
            {
                case (LiquidPhaseState.Dry):
                    if (cellState.fireState != FireState.Inferno)
                        cellState.fireState = FireState.Burning;
                    cellState.changedValues.Add(AlchemyChangeType.Heat);
                    break;
                case (LiquidPhaseState.Oil):
                    IncreaseFire(cellState, 2);
                    cellState.liquidState = LiquidPhaseState.Dry;
                    cellState.changedValues.Add(AlchemyChangeType.Liquid);
                    break;
                default:
                    cellState.gasState = (GasPhaseState)cellState.liquidState;
                    cellState.liquidState = LiquidPhaseState.Dry;
                    cellState.changedValues.Add(AlchemyChangeType.Liquid);
                    cellState.changedValues.Add(AlchemyChangeType.Gas);
                    break;
            }
            InfernoCheck(cellState);
            ShockCheck(cellState);
        }

        void LiquidToSolid(CellAlchemyState cellState)
        {
            switch (cellState.liquidState)
            {
                case (LiquidPhaseState.Dry):

                    break;
                default:
                    cellState.solidState = (SolidPhaseState)cellState.liquidState;
                    cellState.changedValues.Add(AlchemyChangeType.Solid);
                    cellState.liquidState = LiquidPhaseState.Dry;
                    cellState.changedValues.Add(AlchemyChangeType.Liquid);
                    break;

            }
            ShockCheck(cellState);
        }
        void GasToLiquid(CellAlchemyState cellState)
        {
            if (cellState.gasState == GasPhaseState.Dry)
                return;
            cellState.liquidState = (LiquidPhaseState)cellState.gasState;
            cellState.changedValues.Add(AlchemyChangeType.Liquid);
            cellState.gasState = GasPhaseState.Dry;
            ShockCheck(cellState);
        }
        
        void ReduceFireState(CellAlchemyState cellState, int levels = 1)
        {
            for (int i = 0; i < levels; i++)
                if ((int)cellState.fireState > (int)FireState.Dry)
                    cellState.fireState -= 1;
                    cellState.changedValues.Add(AlchemyChangeType.Heat);
        }
        void ReduceFireWithChill(CellAlchemyState cellState, int levels = 1)
        {
            for (int i = 0; i < levels; i++)
                if ((int)cellState.fireState > (int)FireState.Chill)
                    cellState.fireState -= 1;
                    cellState.changedValues.Add(AlchemyChangeType.Heat);
        }
        void IncreaseFire(CellAlchemyState cellState, int levels = 1)
        {
            for (int i = 0; i < levels; i++)
                if ((int)cellState.fireState < (int)FireState.Inferno)
                    cellState.fireState += 1;
                    cellState.changedValues.Add(AlchemyChangeType.Heat);
            InfernoCheck(cellState);
        }

        //Eliminate all phase states if inferno is active
        void InfernoCheck(CellAlchemyState cellState)
        {
            if (cellState.fireState >= FireState.Inferno)
            {
                if (cellState.gasState != GasPhaseState.Dry) { cellState.gasState = GasPhaseState.Dry; cellState.changedValues.Add(AlchemyChangeType.Gas); }
                if (cellState.liquidState != LiquidPhaseState.Dry) { cellState.liquidState = LiquidPhaseState.Dry; cellState.changedValues.Add(AlchemyChangeType.Liquid); }
                if (cellState.solidState != SolidPhaseState.Dry) { cellState.solidState = SolidPhaseState.Dry; cellState.changedValues.Add(AlchemyChangeType.Solid); }
            }
        }

        void ShockCheck(CellAlchemyState cellState)
        {
            if (cellState.shockState == ShockState.Dry)
                return;
            if (cellState.gasState != GasPhaseState.Dry || cellState.liquidState != LiquidPhaseState.Dry || cellState.solidState != SolidPhaseState.Dry)
                cellState.changedValues.Add(AlchemyChangeType.Shock);
            else
                cellState.shockState = ShockState.Dry;
        }

        public void ApplyVFX(CellAlchemyState cellState)
        {
            List<AlchemyChangeType> processedValues = cellState.GetChangedValues();
            //remove previous effect
            foreach (AlchemyChangeType value in processedValues)
            {
                switch (value)
                {
                    case (AlchemyChangeType.Gas):
                        if (cellState.gasEffect != null)
                            Destroy(cellState.gasEffect);           
                        break;
                    case (AlchemyChangeType.Liquid):
                        if (cellState.liquidEffect != null)
                            Destroy(cellState.liquidEffect);
                        break;
                    case (AlchemyChangeType.Solid):
                        if (cellState.solidEffect != null)
                            Destroy(cellState.solidEffect);
                        break;
                    case (AlchemyChangeType.Bless):
                        if (cellState.blessedEffect != null)
                            Destroy(cellState.blessedEffect);
                        break;
                    case (AlchemyChangeType.Heat):
                        if (cellState.fireEffect != null)
                            Destroy(cellState.fireEffect);
                        break;
                    case (AlchemyChangeType.Shock):
                        if (cellState.shockEffect != null)
                            Destroy(cellState.shockEffect);
                        break;
                }
            }
            //instansiate new effect, if applicapable
            foreach (AlchemyChangeType value in processedValues)
            {
                switch (value)
                {
                    case (AlchemyChangeType.Gas):
                        switch (cellState.gasState)
                        {
                            case (GasPhaseState.Water):
                                cellState.gasEffect = Instantiate(WaterGasEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                                break;
                            case (GasPhaseState.Poison):
                                cellState.gasEffect = Instantiate(PoisonGasEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                                break;
                        }
                        break;
                    case (AlchemyChangeType.Liquid):
                        switch (cellState.liquidState)
                        {
                            case (LiquidPhaseState.Water):
                                cellState.liquidEffect = Instantiate(WaterLiquidEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                                break;
                            case (LiquidPhaseState.Poison):
                                cellState.liquidEffect = Instantiate(PoisonLiquidEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                                break;
                            case (LiquidPhaseState.Oil):
                                cellState.liquidEffect = Instantiate(OilLiquidEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                                break;
                        }
                        break;
                    case (AlchemyChangeType.Solid):
                        switch (cellState.solidState)
                        {
                            case (SolidPhaseState.Water):
                                cellState.solidEffect = Instantiate(WaterSolidEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                                break;
                            case (SolidPhaseState.Poison):
                                cellState.solidEffect = Instantiate(PoisonSolidEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                                break;
                        }
                        break;
                    case (AlchemyChangeType.Bless):
                        switch (cellState.blessingState)
                        {
                            case (BlessingState.Blessed):

                                break;
                            case (BlessingState.Cursed):

                                break;
                        }
                        break;
                    case (AlchemyChangeType.Heat):
                        switch (cellState.fireState)
                        {
                            case (FireState.Chill):
                                cellState.fireEffect = Instantiate(ChillEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                                break;
                            case (FireState.Burning):
                                cellState.fireEffect = Instantiate(BurningEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                                break;
                            case (FireState.Inferno):
                                cellState.fireEffect = Instantiate(InfernoEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                                break;
                        }
                        break;
                    case (AlchemyChangeType.Shock):
                        if (cellState.shockState == ShockState.Dry) { 

                        } else if (cellState.solidState != SolidPhaseState.Dry)
                        {
                            cellState.shockEffect = Instantiate(ShockSolidEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                        } else if (cellState.gasState != GasPhaseState.Dry)
                        {
                            cellState.shockEffect = Instantiate(ShockGasEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                        } else if (cellState.liquidState != LiquidPhaseState.Dry)
                        {
                            cellState.shockEffect = Instantiate(ShockLiquidEffectPrefab, cellState.transform.position, cellState.transform.rotation);
                        } else
                        {
                            cellState.shockState = ShockState.Dry;
                        }
                        break;
                }
            }
            cellState.UpdateInternalState();
        }
    }
}
