using CommandChoice.Model;
using UnityEngine;

namespace CommandChoice.Component
{
    public class CameraManager : MonoBehaviour
    {
        //[SerializeField] private Camera camera;
        [SerializeField] private GameObject player;
        [SerializeField] private Vector3 offset;
        [SerializeField] private float smoothTime = 0.25f;
        [SerializeField] private Vector3 velocity = Vector3.zero;
        ZoomComponent zoomComponent;

        void Awake()
        {
            //camera = GetComponent<Camera>();
            player = GameObject.FindGameObjectWithTag(StaticText.TagPlayer);
        }

        void Start()
        {
            zoomComponent = GameObject.Find("Zoom").GetComponent<ZoomComponent>();
        }

        void Update()
        {
            try
            {
                if (!zoomComponent.ZoomActive)
                {
                    Vector3 targetPosition = player.transform.position + offset;
                    transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
                }
            }
            catch (System.Exception)
            {
                zoomComponent = GameObject.Find("Zoom").GetComponent<ZoomComponent>();
            }
        }
    }
}