using UnityEngine;

public class FixCommand : MonoBehaviour
{
    int indexCommand;
    // Start is called before the first frame update
    void Start()
    {
        if (transform.childCount == 0) gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.parent.childCount - 1 != transform.GetSiblingIndex())
        {
            transform.SetAsLastSibling();
        }
    }
}
