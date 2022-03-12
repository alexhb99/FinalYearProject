using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RandIntValue : MonoBehaviour
{
    public int randMin;
    public int randMax;
    public int absoluteMin;
    public int absoluteMax;

    TMP_InputField valInput;
    Button randButton;

    private void Start()
    {
        valInput = transform.GetChild(0).GetComponent<TMP_InputField>();
        randButton = transform.GetChild(1).GetComponent<Button>();
        randButton.onClick.AddListener(delegate { SetRandValue(); });

        SetRandValue();
    }

    public void UpdateValue(string strVal)
    {
        int val;
        bool check = int.TryParse(strVal, out val);
        if (check)
        {
            val = (int)Mathf.Min(absoluteMax, Mathf.Max(absoluteMin, int.Parse(strVal)));
            valInput.text = val.ToString();
        }
    }

    public void SetRandValue()
    {
        valInput.text = Random.Range(randMin, randMax).ToString();
    }

    public int GetValue()
    {
        return int.Parse(valInput.text);
    }
}
