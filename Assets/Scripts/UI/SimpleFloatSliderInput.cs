using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SimpleFloatSliderInput : MonoBehaviour
{
    public float defaultVal;
    public float absoluteMin;
    public float absoluteMax;

    private TMP_InputField valInput;
    [HideInInspector]
    public Slider slider;

    private void Start()
    {
        slider = transform.GetChild(0).GetComponent<Slider>();
        valInput = transform.GetChild(1).GetComponent<TMP_InputField>();

        valInput.text = defaultVal.ToString("F2");
        slider.maxValue = absoluteMax;
        slider.minValue = absoluteMin;
        slider.value = defaultVal;
    }

    public void UpdateSliderValue(string strValue)
    {
        float val = float.Parse(strValue);
        slider.value = Mathf.Min(slider.maxValue, Mathf.Max(slider.minValue, val));
        valInput.text = slider.value.ToString();
    }

    public void UpdateFromSlider(float val)
    {
        valInput.text = val.ToString("F2");
    }

    public float GetValue()
    {
        return slider.value;
    }
}
