using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace PrototypeGame
{
    public class HeightSlider : MonoBehaviour
    {
        public Text valueText;
        Slider slider;

        public InputField minField;
        public InputField maxField;
        private void Start()
        {
            slider = GetComponent<Slider>();
            slider.onValueChanged.AddListener(delegate { ValueChange(); });
            ValueChange();
            minField.onEndEdit.AddListener(delegate { MinChange(); });
            minField.text = slider.minValue.ToString();
            maxField.onEndEdit.AddListener(delegate { MaxChange(); });
            maxField.text = slider.maxValue.ToString();
        }

        void ValueChange()
        {
            valueText.text = slider.value.ToString();
        }

        void MinChange()
        {
            float hold = slider.minValue;
            try
            {
                slider.minValue = float.Parse(minField.text);
            }
            catch (FormatException)
            {
                slider.minValue = hold;
                minField.text = hold.ToString();
            }
        }

        void MaxChange()
        {
            float hold = slider.maxValue;
            try
            {
                slider.maxValue = float.Parse(maxField.text);
            }
            catch (FormatException)
            {
                slider.maxValue = hold;
                maxField.text = hold.ToString();
            }
        }
    }

}
