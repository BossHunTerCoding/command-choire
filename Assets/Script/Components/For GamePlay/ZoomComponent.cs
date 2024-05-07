using CommandChoice.Model;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class ZoomComponent : MonoBehaviour
    {
        [Header("Fields Auto Set")]
        [SerializeField] Button button;
        [SerializeField] Image image;
        [SerializeField] Camera Camera;
        [field: SerializeField] public bool ZoomActive { get; private set; }
        [SerializeField] float zoomSmooth = 8f;
        [SerializeField] float minZoom = 3f;
        [SerializeField] float maxZoom = 6f;
        [SerializeField] bool finishZoom;
        [SerializeField] private Vector2 touchPosition0, touchPosition1;
        [SerializeField] private float previousDistance;

        void Awake()
        {
            ZoomActive = false;
            Camera = Camera.main;
            button = GetComponent<Button>();
            image = transform.GetChild(0).GetComponent<Image>();
        }

        void Start()
        {
            button.onClick.AddListener(() => OnClickActiveZoom());
        }

        void Update()
        {
            if (Input.GetMouseButton(3)) { }
            if (ZoomActive)
            {
                if (Camera.orthographicSize <= maxZoom && !finishZoom)
                {
                    Camera.orthographicSize += Time.deltaTime * zoomSmooth;
                };
                if (Camera.orthographicSize > maxZoom) { finishZoom = true; }
                // if (Input.touchCount == 2 && finishZoom && Camera.GetComponent<CameraManager>().onScreen)
                // {
                //     touchPosition0 = Input.GetTouch(0).deltaPosition;
                //     touchPosition1 = Input.GetTouch(1).deltaPosition;
                //     float currentDistance = Vector2.Distance(touchPosition0, touchPosition1);
                //     float zoomFactor = currentDistance - previousDistance;

                //     // Adjust camera based on zoom direction and sensitivity
                //     Camera.orthographicSize += zoomFactor * (zoomSmooth / 2); // Adjust sensitivity as needed

                //     // Implement zoom limits here if desired
                //     Camera.orthographicSize = Mathf.Clamp(Camera.orthographicSize, minZoom, maxZoom);

                //     previousDistance = currentDistance;
                // }
            }
            else if (!ZoomActive)
            {
                if (Camera.orthographicSize > minZoom) { Camera.orthographicSize -= Time.deltaTime * zoomSmooth; finishZoom = false; }
                touchPosition0 = Vector2.zero;
                touchPosition1 = Vector2.zero;
            }
        }

        public void OnClickActiveZoom()
        {
            ZoomActive = !ZoomActive;
            CheckScreenSize();
            if (ZoomActive)
            {
                image.sprite = Resources.Load<Sprite>(StaticText.PathImgMinimize);
            }
            else
            {
                image.sprite = Resources.Load<Sprite>(StaticText.PathImgZoom);
            }
        }

        void CheckScreenSize()
        {
            if (Screen.height >= 1500f) { maxZoom = 9.25f; minZoom = 4.25f; }
            if (Screen.height <= 900f) { maxZoom = 6.9f; minZoom = 3.5f; }
            else { maxZoom = 6f; minZoom = 3f; }
            // print(Screen.height + " " + Screen.width);
            // print("Platform" + Application.platform);
        }
    }
}