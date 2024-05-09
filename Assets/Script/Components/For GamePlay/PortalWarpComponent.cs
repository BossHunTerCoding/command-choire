using CommandChoice.Model;
using UnityEngine;

public class PortalWarpComponent : MonoBehaviour
{
    [SerializeField] PortalWarpPositionSpawn[] RandomPosition;
    public GameObject LinkPortal { get; private set; }

    void Start()
    {
        try
        {
            if (LinkPortal == null)
            {
                int indexSpawn = Random.Range(0, RandomPosition.Length - 1);
                transform.position = RandomPosition[indexSpawn].Portal1.position;
                LinkPortal = Instantiate(gameObject, transform.parent);
                LinkPortal.GetComponent<PortalWarpComponent>().SetLinkPortal(gameObject);
                LinkPortal.transform.position = RandomPosition[indexSpawn].Portal2.position;
            }
        }
        catch (System.Exception)
        {
        }
    }

    public void SetLinkPortal(GameObject gameObject)
    {
        LinkPortal = gameObject;
    }
}

[System.Serializable]
public class PortalWarpPositionSpawn
{
    [field: SerializeField] public Transform Portal1 { get; private set; }
    [field: SerializeField] public Transform Portal2 { get; private set; }
}
