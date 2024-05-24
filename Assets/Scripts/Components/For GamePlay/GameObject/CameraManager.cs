using System;
using UnityEngine;
using UnityEngine.UI;
using CommandChoice.Data;
using CommandChoice.Model;

namespace CommandChoice.Component
{
    public class CameraManager : MonoBehaviour
    {
        [Header("Camera Settings")]
        [SerializeField] private Camera mainCamera;
        [SerializeField] private GameObject player;
        [SerializeField] private Vector3 offset;

        [Header("Input Settings")]
        [SerializeField] private float smoothTime = 0.25f;
        public Vector3 velocity = Vector3.zero;
        [SerializeField] private Vector2 inputPosition;
        private Vector3 previousMousePosition;

        [Header("Zoom Settings")]
        [SerializeField] private ZoomComponent zoomComponent;

        [Header("Drag Settings")]
        [SerializeField] private Bounds cameraBounds;
        [SerializeField] private SpriteRenderer worldBoundsRenderer;
        [SerializeField] private float height;
        [SerializeField] private float width;
        [field: SerializeField] public Vector2 BoundsMin { get; private set; }
        [field: SerializeField] public Vector2 BoundsExtents { get; private set; }

        private void Awake()
        {
            gameObject.tag = StaticText.TagCamera;
            mainCamera = Camera.main;
            player = GameObject.FindGameObjectWithTag(StaticText.TagPlayer);
            worldBoundsRenderer = worldBoundsRenderer.GetComponent<SpriteRenderer>();
            zoomComponent.ZoomActive = false;
        }

        private void Start()
        {
            InitializeZoomComponent();
        }

        private void Update()
        {
            UpdateBounds();
            HandleInput();
            AdjustZoom();
        }

        void LateUpdate()
        {
            MoveCamera();
        }

        private void InitializeZoomComponent()
        {
            zoomComponent.button = GameObject.Find("Zoom").GetComponent<Button>();
            zoomComponent.image = GameObject.Find("Zoom").transform.GetChild(0).GetComponent<Image>();
            zoomComponent.button.onClick.AddListener(() => zoomComponent.ToggleZoom());
        }

        private void HandleInput()
        {
            if (AreaDragComponent.onScreen)
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    if (previousMousePosition != Vector3.zero) inputPosition = currentMousePosition - previousMousePosition;
                    previousMousePosition = currentMousePosition;  // Update for next frame
                    //print("Mouse");
                }
                else if (Input.touchCount > 0)
                {
                    inputPosition = Input.GetTouch(0).deltaPosition;
                    //print("Mobile");
                }
            }
            else
            {
                previousMousePosition = Vector3.zero;
                inputPosition = Vector3.zero;
            }
        }

        private void MoveCamera()
        {
            Vector3 targetPosition = GetTargetCameraPosition(zoomComponent.ZoomActive);
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }

        private void AdjustZoom()
        {
            if (!zoomComponent.ZoomActive)
            {
                if (mainCamera.orthographicSize > zoomComponent.minZoom)
                {
                    mainCamera.orthographicSize -= Time.deltaTime * zoomComponent.zoomSmooth;
                    zoomComponent.finishZoom = false;
                }
            }
            else
            {
                if (mainCamera.orthographicSize <= zoomComponent.maxZoom && !zoomComponent.finishZoom)
                {
                    mainCamera.orthographicSize += Time.deltaTime * zoomComponent.zoomSmooth;
                }
                if (mainCamera.orthographicSize > zoomComponent.maxZoom)
                {
                    zoomComponent.finishZoom = true;
                }
            }
        }

        public void UpdateBounds()
        {
            height = mainCamera.orthographicSize;
            width = height * mainCamera.aspect;
            BoundsMin = new Vector2(worldBoundsRenderer.bounds.min.x + width, worldBoundsRenderer.bounds.min.y + height);
            BoundsExtents = new Vector2(worldBoundsRenderer.bounds.max.x - width, worldBoundsRenderer.bounds.max.y - height);
            cameraBounds = new Bounds();
            cameraBounds.SetMinMax(BoundsMin, BoundsExtents);
        }

        private Vector3 GetTargetCameraPosition(bool unlockPlayer)
        {
            Vector3 targetPosition = transform.position;

            if (unlockPlayer)
            {
                targetPosition += new Vector3(inputPosition.x * DataGlobal.SettingGame.SensitiveCam * Time.deltaTime,
                                              inputPosition.y * DataGlobal.SettingGame.SensitiveCam * Time.deltaTime, 0f);
            }
            else
            {
                targetPosition = player.transform.position + offset;
            }

            return new Vector3(
                Mathf.Clamp(targetPosition.x, cameraBounds.min.x, cameraBounds.max.x),
                Mathf.Clamp(targetPosition.y, cameraBounds.min.y, cameraBounds.max.y),
                transform.position.z);
        }
    }

    [Serializable]
    public class ZoomComponent
    {
        [Header("Fields Auto Set")]
        public Button button;
        public Image image;
        public bool ZoomActive = false;
        public float zoomSmooth = 8f;
        public float minZoom = 3f;
        public float maxZoom = 6f;
        public bool finishZoom;
        public Vector2 touchPosition0, touchPosition1;
        private float previousDistance;

        public void ToggleZoom()
        {
            ZoomActive = !ZoomActive;
            UpdateZoomImage();
        }

        private void UpdateZoomImage()
        {
            string imagePath = ZoomActive ? StaticText.PathImgMinimize : StaticText.PathImgZoom;
            image.sprite = Resources.Load<Sprite>(imagePath);
        }

        private void CheckScreenSize()
        {
            if (Screen.height >= 1500f)
            {
                maxZoom = 9.25f;
                minZoom = 4.25f;
            }
            else if (Screen.height <= 900f)
            {
                maxZoom = 6.9f;
                minZoom = 3.5f;
            }
            else
            {
                maxZoom = 6f;
                minZoom = 3f;
            }
        }
    }
}
