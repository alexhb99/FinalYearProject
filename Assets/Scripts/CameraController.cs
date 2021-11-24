using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera cam;
    private Vector3 dragOffset;
    private Vector3 dragOrigin;
    public float speed;
    public float scrollSpeed;
    public float dragSpeed;
    private float targetSize;
    public Vector2 camZoomBounds;

    private void Start()
    {
        cam = Camera.main;
    }

    private void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, transform.position + dragOffset, Time.deltaTime * speed);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(2))
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.GetMouseButton(2))
        {
            dragOffset = -(cam.ScreenToWorldPoint(Input.mousePosition) - dragOrigin);
            dragOffset *= dragSpeed;
        }
        else
        {
            dragOffset = Vector3.zero;
        }

        targetSize = Mathf.Clamp(targetSize - (Input.mouseScrollDelta.y * 8 * cam.orthographicSize / camZoomBounds.y), camZoomBounds.x, camZoomBounds.y);
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * scrollSpeed);
    }
}
