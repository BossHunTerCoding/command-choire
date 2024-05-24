using System;
using System.Collections.Generic;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CommandChoice.Component
{
    public class BackgroundDropCommand : MonoBehaviour, IDropHandler
    {

        [field: SerializeField] public GameObject RootContentCommand { get; private set; }
        [field: SerializeField] public CommandManager CommandManager { get; private set; }

        void Awake()
        {
            RootContentCommand = GameObject.FindGameObjectWithTag(StaticText.RootListContentCommand);
            CommandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
        }

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if (!DataGlobal.GamePlay.playActionCommand)
            {
                List<float> minPositions = new List<float>();
                Transform dropObject = eventData.pointerDrag.transform;
                if (dropObject.gameObject.tag != StaticText.TagCommand) return;
                List<Transform> ListCommandSelected = new();
                Transform transformObject = GameObject.FindGameObjectWithTag(StaticText.RootListContentCommand).transform;
                foreach (Transform parent in transformObject)
                {
                    if (StaticText.CheckCommand(parent.gameObject.name)) ListCommandSelected.Add(parent);
                }
                foreach (var item in ListCommandSelected)
                {
                    minPositions.Add(Math.Abs(item.position.y - dropObject.position.y));
                }

                float? index = null;
                Transform point = null;
                for (int i = 0; i < minPositions.Count; i++)
                {
                    if (index == null)
                    {
                        index = minPositions[i];
                        point = ListCommandSelected[i];
                        continue;
                    }
                    if (minPositions[i] < index)
                    {
                        index = minPositions[i];
                        point = ListCommandSelected[i];
                    }
                }
                if (point == null) return;
                Command dropCommandObject = dropObject.GetComponent<Command>();
                // print(dropCommandObject.transform.position.y - point.position.y);
                dropCommandObject.Parent.UpdateParent(point.parent);
                if (point.position.y >= dropCommandObject.transform.position.y)
                {
                    dropCommandObject.Parent.UpdateIndex(point.GetSiblingIndex() + 1);
                }
                else
                {
                    dropCommandObject.Parent.UpdateIndex(point.GetSiblingIndex());
                }
            }
        }
    }
}