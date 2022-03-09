using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSourceGhost : MonoBehaviour
{
    public GameObject prefab;

    private GridController gridController;
    private Transform foodParent;

    private SpriteRenderer spriteRenderer;
    private bool canPlace;
    private Color positiveColour = new Color(0.117f, 1f, 0f, 0.7843137f);
    private Color negativeColour = new Color(0.752f, 0.213f, 0.1366809f, 0.7843137f);

    IntSliderInput nutrition;

    private void Start()
    {
        canPlace = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        gridController = GameObject.FindWithTag("Pathfinding").GetComponent<GridController>();
        foodParent = GameObject.FindWithTag("FoodParent").transform;

        UIController ui = GameObject.FindWithTag("SimulationController").GetComponent<UIController>();
        nutrition = ui.foodSourceCreator.transform.GetChild(2).GetComponent<IntSliderInput>();

        if (nutrition.slider.value < 1)
        {
            transform.localScale = Vector3.one;
        }
        else
        {
            Vector3 scalar = new Vector3(Mathf.Min(10f, nutrition.slider.value / 10f - 1), Mathf.Min(10f, nutrition.slider.value / 10f - 1), 0);
            transform.localScale = Vector3.one + scalar;
        }
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
            GameObject instance = Instantiate(prefab, (Vector2)transform.position, Quaternion.identity, foodParent);
            instance.GetComponent<FoodUnit>().Initialize((int)nutrition.slider.value);
        }
        Destroy(gameObject);
    }
}
