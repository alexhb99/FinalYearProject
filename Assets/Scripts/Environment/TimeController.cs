using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public float time;
    public float daySeconds;
    public float sunriseTime;
    public float sunsetTime;

    private bool doneSunrise;
    private bool doneSunset;
    private int days;

    private Animator lightAC;

    private void Start()
    {
        doneSunrise = true;
        doneSunset = false;
        days = 0;

        //Start at midday!
        time = daySeconds / 2f;

        lightAC = GetComponent<Animator>();
    }

    private void Update()
    {
        lightAC.speed = TimeControls.timeScale;
        Tick();
    }

    private void Tick()
    {
        time += Time.deltaTime * TimeControls.timeScale;

        if(!doneSunset && time > daySeconds * sunsetTime)
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
        }
    }
}
