using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class AddCommandButton : MonoBehaviour
    {
        [SerializeField] GameObject menuListCommand;

        void Start()
        {
            gameObject.GetComponent<Button>().onClick.AddListener(() =>
            {
                Transform transform = GameObject.FindGameObjectWithTag(StaticText.TagCanvas).transform;
                Instantiate(menuListCommand, transform);
            });
        }
    }
}
