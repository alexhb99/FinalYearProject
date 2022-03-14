using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    public GameObject antColonyGhost;
    public GameObject foodSourceGhost;

    public GameObject antColonyCreator;
    public GameObject foodSourceCreator;

    private List<GameObject> creatorMenus;

    private SimulationController simulationController;
    private TimeController timeController;

    private GameObject setupInterface;
    private GameObject closeIcon;
    private Vector2Input worldSize;
    private RandIntValue seed;
    private FloatInput scale;
    private IntInput octaves;
    private SimpleFloatSliderInput persistance;
    private FloatInput lacunarity;

    private FloatInput lengthOfDay;
    private FloatInput sunriseTime;
    private FloatInput sunsetTime;

    private void Start()
    {
        creatorMenus = new List<GameObject>();
        creatorMenus.Add(antColonyCreator);
        creatorMenus.Add(foodSourceCreator);

        simulationController = GameObject.FindWithTag("SimulationController").GetComponent<SimulationController>();
        timeController = GameObject.FindWithTag("GlobalLight").GetComponent<TimeController>();

        Transform worldSetup = GameObject.FindWithTag("WorldGenerationSetup").transform;
        setupInterface = worldSetup.parent.gameObject;
        closeIcon = setupInterface.transform.GetChild(1).gameObject;
        closeIcon.SetActive(false);
        worldSize = worldSetup.GetChild(0).GetComponent<Vector2Input>();
        seed = worldSetup.GetChild(1).GetComponent<RandIntValue>();
        scale = worldSetup.GetChild(2).GetComponent<FloatInput>();
        octaves = worldSetup.GetChild(3).GetComponent<IntInput>();
        persistance = worldSetup.GetChild(4).GetComponent<SimpleFloatSliderInput>();
        lacunarity = worldSetup.GetChild(5).GetComponent<FloatInput>();

        Transform dayNightCycleSetup = GameObject.FindWithTag("DayNightCycleSetup").transform;
        lengthOfDay = dayNightCycleSetup.GetChild(0).GetComponent<FloatInput>();
        sunriseTime = dayNightCycleSetup.GetChild(1).GetComponent<FloatInput>();
        sunsetTime = dayNightCycleSetup.GetChild(2).GetComponent<FloatInput>();

        foreach (GameObject creatorMenu in creatorMenus)
        {
            creatorMenu.SetActive(false);
        }
    }

    public void InstantiateGhost(string ghostType)
    {
        GameObject ghostPrefab = (GameObject)GetType().GetField(ghostType + "Ghost", BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance).GetValue(this);
        Instantiate(ghostPrefab, (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

    public void ToggleCreatorMenu(GameObject uiElement)
    {
        foreach(GameObject creatorMenu in creatorMenus)
        {
            creatorMenu.SetActive(uiElement.name == creatorMenu.name ? !uiElement.activeInHierarchy : false);
        }
    }

    public void BeginSimulation()
    {
        timeController.SetParameters(lengthOfDay.GetValue(), sunriseTime.GetValue(), sunsetTime.GetValue());

        simulationController.StartSimulation(worldSize.GetVector2Int(), seed.GetValue(), scale.GetValue(), octaves.GetValue(), persistance.GetValue(), lacunarity.GetValue());
        closeIcon.SetActive(true);
        setupInterface.SetActive(false);
    }

    public void ToggleSetupSimulation()
    {
        setupInterface.SetActive(!setupInterface.activeInHierarchy);
    }
}
