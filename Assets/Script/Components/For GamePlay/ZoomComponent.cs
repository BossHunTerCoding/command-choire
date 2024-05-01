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
        [SerializeField] float smoothTime = 0.25f;
        Vector3 Velocity;
        [SerializeField] Vector3 vectorPosition;
        float? maxTop = null;
        float? maxBottom = null;
        float? maxLeft = null;
        float? maxRight = null;

        void Awake()
        {
            ZoomActive = false;
            Camera = GameObject.FindGameObjectWithTag(StaticText.TagCamera).GetComponent<Camera>();
            button = GetComponent<Button>();
            image = transform.GetChild(0).GetComponent<Image>();
        }

        void Start()
        {
            foreach (GameObject item in FindObjectsOfType<GameObject>())
            {
                if (item.layer == LayerMask.NameToLayer("Wall"))
                {
                    maxRight ??= item.transform.position.x;
                    maxLeft ??= item.transform.position.x;
                    maxBottom ??= item.transform.position.y;
                    maxTop ??= item.transform.position.y;
                    if (item.transform.position.x > maxRight) maxRight = item.transform.position.x;
                    if (item.transform.position.x < maxLeft) maxLeft = item.transform.position.x;
                    if (item.transform.position.y > maxTop) maxTop = item.transform.position.y;
                    if (item.transform.position.y < maxBottom) maxBottom = item.transform.position.y;
                }
            }
            button.onClick.AddListener(() => OnClickActiveZoom());
        }

        void Update()
        {
            if (ZoomActive)
            {
                vectorPosition = new(((maxLeft ?? 0 + maxRight ?? 0) / 2) + maxLeft / 2 ?? 0, ((maxBottom ?? 0 + maxTop ?? 0) / 2) + maxBottom - 2f ?? 0, Camera.transform.position.z);
                if (Camera.orthographicSize <= maxZoom) Camera.orthographicSize += Time.deltaTime * zoomSmooth;
                Camera.transform.position = Vector3.SmoothDamp(Camera.transform.position, vectorPosition, ref Velocity, smoothTime);
            }
            else if (!ZoomActive)
            {
                if (Camera.orthographicSize > minZoom) Camera.orthographicSize -= Time.deltaTime * zoomSmooth;
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