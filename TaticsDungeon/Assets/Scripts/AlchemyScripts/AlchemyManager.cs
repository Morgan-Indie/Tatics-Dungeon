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

    public class AlchemyManager : MonoBehaviour
    {
        enum ChangeValues { Gas, Liquid, Solid, Blessed, Fire, Shock};
        List<ChangeValues> changeValues;

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

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            changeValues = new List<ChangeValues>();
        }

        public void ApplyGas(CellAlchemyState cellState, GasPhaseState gas)
        {
            cellState.gasState = gas;
            changeValues.Add(ChangeValues.Gas);
            ShockCheck(cellState);
            ApplyVFX(cellState);
        }

        public void ApplyLiquid(CellAlchemyState cellState, LiquidPhaseState liquid)
        {
            cellState.liquidState = liquid;
            changeValues.Add(ChangeValues.Liquid);
            if ((int)cellState.fireState > (int)FireState.Dry)
            {
                ApplyHeatInternal(cellState);
                if (liquid != LiquidPhaseState.Oil)
                    ReduceFireState(cellState);
            } else if ((int)cellState.fireState < (int)FireState.Dry)
            {
                ApplyChillInternal(cellState);
            }
            ShockCheck(cellState);
            ApplyVFX(cellState);
        }

        public void ApplySolid(CellAlchemyState cellState, SolidPhaseState solid)
        {
            cellState.solidState = solid;
            changeValues.Add(ChangeValues.Solid);
            if ((int)cellState.fireState > (int)FireState.Dry)
            {
                ApplyHeatInternal(cellState, 2);
            }
            ShockCheck(cellState);
            ApplyVFX(cellState);
        }

        public void ApplyHeat(CellAlchemyState cellState)
        {            
            ApplyHeatInternal(cellState);
            ApplyVFX(cellState);
        }

        void ApplyHeatInternal(CellAlchemyState cellState, int itters = 1)
        {
            LiquidToGas(cellState);
            SolidToLiquid(cellState);
            if (itters - 1 > 0)
                ApplyHeatInternal(cellState, itters - 1);
        }

        public void ApplyChill(CellAlchemyState cellState)
        {
            ApplyChillInternal(cellState);
            if (changeValues.Contains(ChangeValues.Solid) || changeValues.Contains(ChangeValues.Liquid) && cellState.fireState == FireState.Chill)
                cellState.fireState = FireState.Dry;
            ApplyVFX(cellState);
        }

        void ApplyChillInternal(CellAlchemyState cellState, int itters = 1)
        {
            LiquidToSolid(cellState);
            GasToLiquid(cellState);
            ReduceFireWithChill(cellState);
            if (itters - 1 > 0)
                ApplyChillInternal(cellState, itters - 1);
        }

        public void ApplyShock(CellAlchemyState cellState)
        {
            cellState.shockState = ShockState.Shocked;
            changeValues.Add(ChangeValues.Shock);
            ApplyVFX(cellState);
        }

        void SolidToLiquid(CellAlchemyState cellState)
        {
            if (cellState.solidState == SolidPhaseState.Dry)
                return;
            cellState.liquidState = (LiquidPhaseState)cellState.solidState;
            changeValues.Add(ChangeValues.Liquid);
            ReduceFireState(cellState);
            cellState.solidState = SolidPhaseState.Dry;
            changeValues.Add(ChangeValues.Solid);
            ShockCheck(cellState);
        }
        void LiquidToGas(CellAlchemyState cellState)
        {
            switch (cellState.liquidState)
            {
                case (LiquidPhaseState.Dry):
                    if (cellState.fireState != FireState.Inferno)
                        cellState.fireState = FireState.Burning;
                    changeValues.Add(ChangeValues.Fire);
                    break;
                case (LiquidPhaseState.Oil):
                    IncreaseFire(cellState, 2);
                    cellState.liquidState = LiquidPhaseState.Dry;
                    changeValues.Add(ChangeValues.Liquid);
                    break;
                default:
                    cellState.gasState = (GasPhaseState)cellState.liquidState;
                    cellState.liquidState = LiquidPhaseState.Dry;
                    changeValues.Add(ChangeValues.Liquid);
                    changeValues.Add(ChangeValues.Gas);
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
                    changeValues.Add(ChangeValues.Solid);
                    cellState.liquidState = LiquidPhaseState.Dry;
                    changeValues.Add(ChangeValues.Liquid);
                    break;

            }
            ShockCheck(cellState);
        }
        void GasToLiquid(CellAlchemyState cellState)
        {
            if (cellState.gasState == GasPhaseState.Dry)
                return;
            cellState.liquidState = (LiquidPhaseState)cellState.gasState;
            changeValues.Add(ChangeValues.Liquid);
            cellState.gasState = GasPhaseState.Dry;
            ShockCheck(cellState);
        }
        
        void ReduceFireState(CellAlchemyState cellState, int levels = 1)
        {
            for (int i = 0; i < levels; i++)
            {
                if ((int)cellState.fireState > (int)FireState.Dry)
                {
                    cellState.fireState -= 1;
                    changeValues.Add(ChangeValues.Fire);
                }
            }
        }
        void ReduceFireWithChill(CellAlchemyState cellState, int levels = 1)
        {
            for (int i = 0; i < levels; i++)
            {
                if ((int)cellState.fireState > (int)FireState.Chill)
                {
                    cellState.fireState -= 1;
                    changeValues.Add(ChangeValues.Fire);
                }
            }
        }
        void IncreaseFire(CellAlchemyState cellState, int levels = 1)
        {
            for (int i = 0; i < levels; i++)
            {
                if ((int)cellState.fireState < (int)FireState.Inferno)
                {
                    cellState.fireState += 1;
                    changeValues.Add(ChangeValues.Fire);
                }
            }
            InfernoCheck(cellState);
        }

        //Eliminate all phase states if inferno is active
        void InfernoCheck(CellAlchemyState cellState)
        {
            if (cellState.fireState >= FireState.Inferno)
            {
                if (cellState.gasState != GasPhaseState.Dry) { cellState.gasState = GasPhaseState.Dry; changeValues.Add(ChangeValues.Gas); }
                if (cellState.liquidState != LiquidPhaseState.Dry) { cellState.liquidState = LiquidPhaseState.Dry; changeValues.Add(ChangeValues.Liquid); }
                if (cellState.solidState != SolidPhaseState.Dry) { cellState.solidState = SolidPhaseState.Dry; changeValues.Add(ChangeValues.Solid); }
            }
        }

        void ShockCheck(CellAlchemyState cellState)
        {
            if (cellState.shockState == ShockState.Dry)
                return;
            if (cellState.gasState != GasPhaseState.Dry || cellState.liquidState != LiquidPhaseState.Dry || cellState.solidState != SolidPhaseState.Dry)
                changeValues.Add(ChangeValues.Shock);
            else
                cellState.shockState = ShockState.Dry;
        }

        public void ApplyVFX(CellAlchemyState cellState)
        {
            List<ChangeValues> processedValues = new List<ChangeValues>();
            //remove duplicates
            foreach (ChangeValues value in changeValues) { if (!processedValues.Contains(value)){ processedValues.Add(value); } }
            //remove previous effect
            foreach (ChangeValues value in processedValues)
            {
                switch (value)
                {
                    case (ChangeValues.Gas):
                        if (cellState.gasEffect != null)
                            Destroy(cellState.gasEffect);           
                        break;
                    case (ChangeValues.Liquid):
                        if (cellState.liquidEffect != null)
                            Destroy(cellState.liquidEffect);
                        break;
                    case (ChangeValues.Solid):
                        if (cellState.solidEffect != null)
                            Destroy(cellState.solidEffect);
                        break;
                    case (ChangeValues.Blessed):
                        if (cellState.blessedEffect != null)
                            Destroy(cellState.blessedEffect);
                        break;
                    case (ChangeValues.Fire):
                        if (cellState.fireEffect != null)
                            Destroy(cellState.fireEffect);
                        break;
                    case (ChangeValues.Shock):
                        if (cellState.shockEffect != null)
                            Destroy(cellState.shockEffect);
                        break;
                }
            }
            //instansiate new effect, if applicapable
            foreach (ChangeValues value in processedValues)
            {
                switch (value)
                {
                    case (ChangeValues.Gas):
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
                    case (ChangeValues.Liquid):
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
                    case (ChangeValues.Solid):
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
                    case (ChangeValues.Blessed):
                        switch (cellState.blessingState)
                        {
                            case (BlessingState.Blessed):

                                break;
                            case (BlessingState.Cursed):

                                break;
                        }
                        break;
                    case (ChangeValues.Fire):
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
                    case (ChangeValues.Shock):
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
        }
    }
}
