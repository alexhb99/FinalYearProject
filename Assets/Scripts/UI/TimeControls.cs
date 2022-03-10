using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimeControls : MonoBehaviour
{
    public float defaultTime;
    public float minTime;
    public float maxTime;

    public Sprite pausedIcon;
    public Sprite playIcon;

    private Image pausePlay;
    private Slider slider;
    private TMP_InputField inputField;

    private bool isPaused;
    private float localTimeScale;

    //All scripts access this variable
    public static float timeScale;

    private void Start()
    {
        pausePlay = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        slider = transform.GetChild(1).GetComponent<Slider>();
        inputField = transform.GetChild(2).GetComponent<TMP_InputField>();

        localTimeScale = defaultTime;
        slider.value = localTimeScale;
        inputField.text = localTimeScale.ToString("F2");

        slider.maxValue = maxTime;
        slider.minValue = minTime;
        slider.value = localTimeScale;

        SetTimescale();
    }

    public void UpdateSliderValue(string strValue)
    {
        localTimeScale = Mathf.Min(maxTime, Mathf.Max(minTime, float.Parse(strValue)));
        slider.value = localTimeScale;
        inputField.text = localTimeScale.ToString("F2");
        SetTimescale();
    }

    public void UpdateFromSlider(float val)
    {
        localTimeScale = val;
        inputField.text = localTimeScale.ToString("F2");
        SetTimescale();
    }

    public void TogglePause()
    {
        isPaused = !isPaused;
        pausePlay.sprite = isPaused ? pausedIcon : playIcon;
        SetTimescale();
    }

    private void SetTimescale()
    {
        timeScale = isPaused ? 0 : localTimeScale;
    }
}