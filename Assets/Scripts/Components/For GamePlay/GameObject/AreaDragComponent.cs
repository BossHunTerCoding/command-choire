using UnityEngine;
using UnityEngine.EventSystems;

public class AreaDragComponent : MonoBehaviour
{
    private RectTransform targetRectTransform;

    public static bool onScreen = false;

    void Start()
    {
        targetRectTransform = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        // Check if the left mouse button is clicked or there's any touch
        if (Input.GetMouseButton(0) || Input.touchCount > 0)
        {
            Vector2 inputPosition;
            bool isTouch = Input.touchCount > 0;

            if (isTouch)
            {
                // Use the first touch position
                inputPosition = Input.GetTouch(0).position;
            }
            else
            {
                // Use the mouse position
                inputPosition = Input.mousePosition;
            }

            // Check if the input position is inside the target RectTransform
            bool isInsideTarget = RectTransformUtility.RectangleContainsScreenPoint(targetRectTransform, inputPosition, null);

            // Check if the input position is over any UI element
            bool isOverUI = isTouch ? EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) : EventSystem.current.IsPointerOverGameObject();

            onScreen = isInsideTarget && isOverUI;
        }
        else
        {
            onScreen = false;
        }

        //print(onScreen); // Uncomment for debugging
    }
}