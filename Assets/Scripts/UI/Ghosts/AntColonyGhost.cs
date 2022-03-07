using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntColonyGhost : MonoBehaviour
{
    public GameObject antColonyPrefab;

    private GridController gridController;
    private Transform creatureParent;

    private SpriteRenderer spriteRenderer;
    private bool canPlace;
    private Color positiveColour = new Color(0.117f, 1f, 0f, 0.7843137f);
    private Color negativeColour = new Color(0.752f, 0.213f, 0.1366809f, 0.7843137f);

    IntSliderInput numOfAnts;

    private void Start()
    {
        canPlace = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridController = GameObject.FindWithTag("Pathfinding").GetComponent<GridController>();
        creatureParent = GameObject.FindWithTag("Creatures").transform;

        UIController ui = GameObject.FindWithTag("SimulationController").GetComponent<UIController>();
        numOfAnts = ui.antColonyCreator.transform.GetChild(2).GetComponent<IntSliderInput>();
    }

    private void Update()
    {
        transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (transform.position.x > gridController.gridStartPos.x + 1
            && transform.position.y > gridController.gridStartPos.y + 1
            && transform.position.x < gridController.gridStartPos.x + gridController.size.x - 2
            && transform.position.y < gridController.gridStartPos.y + gridController.size.y - 2)
        {
            canPlace = gridController.GetNodeFromPoint(transform.position).walkable;
        }
        else
        {
            canPlace = false;
        }

        spriteRenderer.color = canPlace ? positiveColour : negativeColour;

        if (Input.GetMouseButtonUp(0))
        {
            PlaceObject();
        }
    }

    private void PlaceObject()
    {
        if (canPlace)
        {
            GameObject instance = Instantiate(antColonyPrefab, (Vector2)transform.position, Quaternion.identity, creatureParent);
            instance.GetComponent<AntColony>().CreateAnts((int)numOfAnts.slider.value);
        }
        Destroy(gameObject);
    }
}
