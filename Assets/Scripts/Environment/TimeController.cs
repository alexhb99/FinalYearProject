using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using TMPro;

public class TimeController : MonoBehaviour
{
    public float time;
    public float daySeconds;
    public float sunriseTime;
    public float sunsetTime;

    public TMP_Text daysUI;
    public TMP_Text timeUI;

    [HideInInspector]
    public float lightLevel;
    private bool doneSunrise;
    private bool doneSunset;
    private int days;

    private bool canTick;

    private Animator lightAC;
    private Light2D light2D;

    private void Start()
    {
        canTick = false;

        lightAC = GetComponent<Animator>();
        light2D = GetComponent<Light2D>();
    }

    private void Update()
    {
        lightAC.speed = TimeControls.timeScale;
        if (canTick)
        {
            Tick();
        }
    }

    private void Tick()
    {
        time += Time.deltaTime * TimeControls.timeScale;
        lightLevel = Mathf.InverseLerp(0.52549f, 1f, light2D.color.g);    //Get light level from looking at green component of light colour - 0.52549f is the g component at night

        if (!doneSunset && time > daySeconds * sunsetTime)
        {
            lightAC.SetTrigger("Sunset");
            doneSunset = true;
        }
        else if(!doneSunrise && time > daySeconds * sunriseTime)
        {
            lightAC.SetTrigger("Sunrise");
            doneSunrise = true;
        }

        if(time > daySeconds)
        {
            time -= daySeconds;
            doneSunrise = false;
            doneSunset = false;
            days++;

            daysUI.text = "Days: " + days;
        }

        timeUI.text = "Time: " + time.ToString("F2");
    }

    public void SetParameters(float timeInDay, float sunrise, float sunset)
    {
        daySeconds = timeInDay;
        sunriseTime = sunrise;
        sunsetTime = sunset;

        doneSunrise = true;
        doneSunset = false;
        days = 0;

        //Start at midday!
        time = daySeconds / 2f;

        canTick = true;
    }
}
