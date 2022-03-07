using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntSliderInput : MonoBehaviour
{
    public string defaultMin;
    public string defaultVal;
    public string defaultMax;

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
        slider.maxValue = int.Parse(defaultMax);
        slider.minValue = int.Parse(defaultMin);
        slider.value = int.Parse(defaultVal);
    }

    public void UpdateSliderMin(string strValue)
    {
        int val = (int)Mathf.Max(0, Mathf.Min(slider.maxValue, int.Parse(strValue)));
        minInput.text = val.ToString();
        slider.minValue = val;
        valInput.onValueChanged.Invoke(valInput.text);
    }
    public void UpdateSliderValue(string strValue)
    {
        int val = int.Parse(strValue);
        slider.value = Mathf.Min(slider.maxValue, Mathf.Max(slider.minValue, val));
        valInput.text = slider.value.ToString();
    }
    public void UpdateSliderMax(string strValue)
    {
        int val = (int)Mathf.Max(slider.minValue, int.Parse(strValue));
        maxInput.text = val.ToString();
        slider.maxValue = val;
        valInput.onValueChanged.Invoke(valInput.text);
    }
    public void UpdateFromSlider(float floatVal)
    {
        int val = Mathf.RoundToInt(floatVal);
        valInput.text = val.ToString();
    }
}
