using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject antColonyGhost;
    public GameObject foodSourceGhost;

    public GameObject antColonyCreator;
    public GameObject foodSourceCreator;

    private List<GameObject> creatorMenus;

    private void Start()
    {
        creatorMenus = new List<GameObject>();
        creatorMenus.Add(antColonyCreator);
        creatorMenus.Add(foodSourceCreator);

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
}
