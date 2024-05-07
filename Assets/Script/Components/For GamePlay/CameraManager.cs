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
        [SerializeField] Vector2 inputPosition;
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
                    if ((Input.GetTouch(0).deltaPosition != Vector2.zero || Input.GetMouseButton(0)) && onScreen)
                    {
                        transform.position = Vector3.SmoothDamp(transform.position, CameraGetBounds(zoomComponent.ZoomActive), ref velocity, smoothTime);
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
            transform.position = Vector3.SmoothDamp(transform.position, CameraGetBounds(), ref velocity, smoothTime);
        }

        private Vector3 CameraGetBounds(bool? unLockPlayer = null)
        {
            Vector3 targetPosition = transform.position;
            if (unLockPlayer != null)
            {
                if (unLockPlayer ?? false)
                {
                    inputPosition = Input.GetTouch(0).deltaPosition;
                    targetPosition = new(targetPosition.x += inputPosition.x, targetPosition.y += inputPosition.y);
                }
                else
                {
                    targetPosition = player.transform.position + offset;
                }
            }
            return new(
                Mathf.Clamp(targetPosition.x, CameraBound.min.x, CameraBound.max.x),
                Mathf.Clamp(targetPosition.y, CameraBound.min.y, CameraBound.max.y),
                transform.position.z
            );
        }
    }
}