using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;

namespace CommandChoice.Component
{
    public class CameraManager : MonoBehaviour
    {
        [SerializeField] private GameObject player;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float smoothTime = 0.25f;
        public Vector3 velocity = Vector3.zero;
        ZoomComponent zoomComponent;
        [SerializeField] Bounds CameraBound;
        [SerializeField] Camera mainCamera;
        public bool OnDrag;
        public bool onScreen;
        [SerializeField] Vector3 inputPosition;
        [SerializeField] private Vector3 previousMousePosition;
        [SerializeField] SpriteRenderer WorldBounds;
        [SerializeField] float hight;
        [SerializeField] float width;
        [field: SerializeField] public Vector2 boundsMin { get; private set; }
        [field: SerializeField] public Vector2 boundsExtents { get; private set; }

        void Awake()
        {
            WorldBounds = WorldBounds.GetComponent<SpriteRenderer>();
            mainCamera = Camera.main;
            player = GameObject.FindGameObjectWithTag(StaticText.TagPlayer);
        }

        void Start()
        {
            zoomComponent = GameObject.Find("Zoom").GetComponent<ZoomComponent>();
        }

        void Update()
        {
            UpdateBounds();
            try
            {
                if (!zoomComponent.ZoomActive && !OnDrag)
                {
                    inputPosition = Vector2.zero;

                    transform.position = Vector3.SmoothDamp(transform.position, CameraGetBounds(zoomComponent.ZoomActive), ref velocity, smoothTime);
                }
                else
                {
                    if (onScreen && OnDrag)
                    {
                        transform.position = Vector3.SmoothDamp(transform.position, CameraGetBounds(zoomComponent.ZoomActive), ref velocity, smoothTime);
                    }
                    else
                    {
                        transform.position = Vector3.SmoothDamp(transform.position, CameraGetBounds(), ref velocity, smoothTime);
                    }
                }
            }
            catch (System.Exception)
            {
                zoomComponent = GameObject.Find("Zoom").GetComponent<ZoomComponent>();
            }
        }

        public void UpdateBounds()
        {
            hight = mainCamera.orthographicSize;
            width = hight * mainCamera.aspect;
            boundsMin = new(WorldBounds.bounds.min.x + width, WorldBounds.bounds.min.y + hight);
            boundsExtents = new(WorldBounds.bounds.max.x - width, WorldBounds.bounds.max.y - hight);
            CameraBound = new();
            CameraBound.SetMinMax(
                new(boundsMin.x, boundsMin.y, 0f),
                new(boundsExtents.x, boundsExtents.y, 0f)
            );
        }

        private Vector3 CameraGetBounds(bool? unLockPlayer = null)
        {
            Vector3 targetPosition = transform.position;
            if (unLockPlayer != null)
            {
                if (unLockPlayer ?? false)
                {
                    if (SystemInfo.deviceType == DeviceType.Desktop) // Check if left mouse button is held
                    {
                        Vector3 currentMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        inputPosition = (currentMousePosition - previousMousePosition) * DataGlobal.settingGame.SensitiveCam;
                        targetPosition = new Vector3(targetPosition.x + inputPosition.x, targetPosition.y + inputPosition.y, targetPosition.z);
                        previousMousePosition = currentMousePosition;  // Update for next frame
                    }
                    else if (SystemInfo.deviceType == DeviceType.Handheld)
                    {
                        inputPosition = Input.GetTouch(0).deltaPosition * DataGlobal.settingGame.SensitiveCam;
                        targetPosition = new(targetPosition.x += inputPosition.x, targetPosition.y += inputPosition.y);
                    }
                }
                else
                {
                    targetPosition = player.transform.position + offset;
                }
            }
            else
            {
                inputPosition = Vector3.zero;
                targetPosition = new(targetPosition.x += inputPosition.x, targetPosition.y += inputPosition.y);
            }
            return new(
                Mathf.Clamp(targetPosition.x, CameraBound.min.x, CameraBound.max.x),
                Mathf.Clamp(targetPosition.y, CameraBound.min.y, CameraBound.max.y),
                transform.position.z
            );
        }
    }
}