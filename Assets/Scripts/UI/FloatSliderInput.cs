using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatSliderInput : MonoBehaviour
{
    public string defaultMin;
    public string defaultVal;
    public string defaultMax;

    public float absoluteMin;
    public float absoluteMax;

    TMP_InputField minInput;
    TMP_InputField valInput;
    TMP_InputField maxInput;
    [HideInInspector]
    public Slider slider;

    private void Start()
    {
        minInput = transform.GetChild(0).GetComponent<TMP_InputField>();
        valInput = transform.GetChild(1).GetComponent<TMP_InputField>();
        maxInput = transform.GetChild(2).GetComponent<TMP_InputField>();
        slider = transform.GetChild(3).GetComponent<Slider>();

        valInput.text = defaultVal;
        minInput.text = defaultMin;
        maxInput.text = defaultMax;
        slider.maxValue = float.Parse(defaultMax);
        slider.minValue = float.Parse(defaultMin);
        slider.value = float.Parse(defaultVal);
    }

    public void UpdateSliderMin(string strValue)
    {
        float val = Mathf.Max(absoluteMin, Mathf.Min(slider.maxValue, float.Parse(strValue)));
        minInput.text = val.ToString("F2");
        slider.minValue = val;
        valInput.onValueChanged.Invoke(valInput.text);
    }
    public void UpdateSliderValue(string strValue)
    {
        float val = float.Parse(strValue);
        slider.value = Mathf.Min(slider.maxValue, Mathf.Max(slider.minValue, val));
        valInput.text = slider.value.ToString("F2");
    }
    public void UpdateSliderMax(string strValue)
    {
        float val = Mathf.Min(absoluteMax, Mathf.Max(slider.minValue, float.Parse(strValue)));
        maxInput.text = val.ToString("F2");
        slider.maxValue = val;
        valInput.onValueChanged.Invoke(valInput.text);
    }
    public void UpdateFromSlider(float val)
    {
        valInput.text = val.ToString("F2");
    }
}
