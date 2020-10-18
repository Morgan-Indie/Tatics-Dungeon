using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using UnityEngine;

namespace PrototypeGame
{
    public class Stat
    {
        public float Basevalue;
        protected bool isDirty = true;
        protected float _value;
        protected readonly List<StatModifier> statModifiers;
        public readonly ReadOnlyCollection<StatModifier> StatModifiers;

        public float Value
        {
            get
            {
                if (isDirty)
                {
                    _value = CalculateFinalvalue();
                    isDirty = false;
                }
                return _value;
            }
            set { Basevalue = value; isDirty = true; }
        }

        public void PrintModifiers()
        {
            foreach (StatModifier mod in statModifiers)
                Debug.Log("Value: " + mod.Value.ToString() + " Mod Type: " + mod.Type.ToString());
        }

        public Stat()
        {
            statModifiers = new List<StatModifier>();
            StatModifiers = statModifiers.AsReadOnly();
        }

        public Stat(float baseValue) : this()
        {
            Basevalue = baseValue;
        }

        public void AddModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Add(mod);
            statModifiers.Sort(CompareModifierOrder);
        }

        protected int CompareModifierOrder(StatModifier a, StatModifier b)
        {
            if (a.Order < b.Order)
                return -1;
            else if (a.Order > b.Order)
                return 1;

            return 0;
        }
        public void RemoveModifier(StatModifier mod)
        {
            isDirty = true;
            statModifiers.Remove(mod);
        }

        public bool RemoveAllModifiersFromSource(object source)
        {
            bool didRemove = false;
            for (int i = statModifiers.Count - 1; i >= 0; i--)
            {
                if (statModifiers[i].Source == source)
                {
                    isDirty = true;
                    didRemove = true;
                    statModifiers.RemoveAt(i);
                }
            }
            return didRemove;
        }

        public void LevelUp()
        {
            Basevalue += 1;
        }


        protected float CalculateFinalvalue()
        {
            float finalValue = Basevalue;
            float sumPercentAdd = 0;

            for (int i = 0; i < statModifiers.Count; i++)
            {
                StatModifier mod = statModifiers[i];
                if (mod.Type == StatModType.Flat)
                {
                    finalValue += mod.Value;
                }
                else if (mod.Type == StatModType.PercentAdd)
                {
                    sumPercentAdd += mod.Value;
                    if (i + 1 >= statModifiers.Count|| statModifiers[i + 1].Type != StatModType.PercentAdd)
                    {
                        finalValue *= 1 + sumPercentAdd;
                        sumPercentAdd = 0;
                    }
                }
                else if (mod.Type == StatModType.PercentMult)
                {
                    finalValue *= 1 + mod.Value;
                }
            }
            // 12.001f !=12f
            return (float)Math.Round(finalValue, 0);
        }
    }
}
