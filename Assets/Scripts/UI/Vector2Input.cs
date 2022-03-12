using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Vector2Input : MonoBehaviour
{
    public Vector2 defaultVector;
    public Vector2 minVector;
    public Vector2 maxVector;

    private TMP_InputField xInput;
    private TMP_InputField yInput;

    [HideInInspector]
    public Vector2 vector;

    private void Start()
    {
        xInput = transform.GetChild(0).GetComponent<TMP_InputField>();
        yInput = transform.GetChild(1).GetComponent<TMP_InputField>();

        vector = defaultVector;
        xInput.text = defaultVector.x.ToString();
        yInput.text = defaultVector.y.ToString();
    }

    public void UpdateX(string strValue)
    {
        int val = int.Parse(strValue);
        vector.x = Mathf.Min(maxVector.x, Mathf.Max(minVector.x, val));
        xInput.text = vector.x.ToString();
    }

    public void UpdateY(string strValue)
    {
        int val = int.Parse(strValue);
        vector.y = Mathf.Min(maxVector.y, Mathf.Max(minVector.y, val));
        yInput.text = vector.y.ToString();
    }

    public Vector2Int GetVector2Int()
    {
        return new Vector2Int(Mathf.RoundToInt(vector.x), Mathf.RoundToInt(vector.y));
    }
}
