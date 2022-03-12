using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntInput : MonoBehaviour
{
    public int defaultVal;
    public int absoluteMin;
    public int absoluteMax;

    TMP_InputField valInput;

    private void Start()
    {
        valInput = transform.GetChild(0).GetComponent<TMP_InputField>();

        valInput.text = defaultVal.ToString();
    }

    public void UpdateValue(string strVal)
    {
        int val = (int)Mathf.Min(absoluteMax, Mathf.Max(absoluteMin, float.Parse(strVal)));
        valInput.text = val.ToString();
    }

    public int GetValue()
    {
        return int.Parse(valInput.text);
    }
}
