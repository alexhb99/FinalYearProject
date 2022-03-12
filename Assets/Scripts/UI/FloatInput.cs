using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatInput : MonoBehaviour
{
    public float defaultVal;
    public float absoluteMin;
    public float absoluteMax;

    TMP_InputField valInput;

    private void Start()
    {
        valInput = transform.GetChild(0).GetComponent<TMP_InputField>();

        valInput.text = defaultVal.ToString();
    }

    public void UpdateValue(string strVal)
    {
        float val = (float)Mathf.Min(absoluteMax, Mathf.Max(absoluteMin, float.Parse(strVal)));
        valInput.text = val.ToString();
    }

    public float GetValue()
    {
        return float.Parse(valInput.text);
    }
}
