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
            if (RectTransformUtility.RectangleContainsScreenPoint(uiElementRectTransform, Input.mousePosition) && zoomComponent.ZoomActive)
            {
                mainCamera.onScreen = true;
            }
            else { mainCamera.onScreen = false; mainCamera.OnDrag = false; }
            if (SystemInfo.deviceType == DeviceType.Desktop ? Input.GetMouseButton(0) : Input.GetTouch(0).deltaPosition != Vector2.zero) mainCamera.OnDrag = true;
            else mainCamera.OnDrag = false;
        }
        catch (System.Exception)
        {
            zoomComponent = GameObject.Find("Zoom").GetComponent<ZoomComponent>();
        }
    }
}
