using System;
using System.Collections.Generic;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class TutorialComponent : MonoBehaviour
    {
        [SerializeField] List<Button> CloseMenu = new();

        private void Start()
        {
            if (CloseMenu.Count > 0)
            {
                foreach (Button close in CloseMenu)
                {
                    close.onClick.AddListener(() => { Destroy(gameObject); });
                }
            }
        }

        public void PopUpTutorial()
        {
            GameObject menu = Resources.Load<GameObject>("Ui/Menu/Tutorial Board");
            Instantiate(menu, GameObject.FindWithTag(StaticText.TagCanvas).transform);
        }
    }
}
