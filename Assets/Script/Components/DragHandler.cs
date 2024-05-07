using CommandChoice.Component;
using UnityEngine;

public class DragHandler : MonoBehaviour
{
    RectTransform uiElementRectTransform;
    ZoomComponent zoomComponent;
    CameraManager mainCamera;

    void Start()
    {
        mainCamera = Camera.main.GetComponent<CameraManager>();
        uiElementRectTransform = gameObject.GetComponent<RectTransform>();
        zoomComponent = GameObject.Find("Zoom").GetComponent<ZoomComponent>();
    }

    void Update()
    {
        try
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(uiElementRectTransform, Input.mousePosition) && Input.GetTouch(0).deltaPosition != Vector2.zero && zoomComponent.ZoomActive)
            {
                mainCamera.onScreen = true;
                mainCamera.OnDrag = true;
            }
            else { mainCamera.onScreen = false; mainCamera.OnDrag = false; }
        }
        catch (System.Exception)
        {
            zoomComponent = GameObject.Find("Zoom").GetComponent<ZoomComponent>();
        }
    }
}
