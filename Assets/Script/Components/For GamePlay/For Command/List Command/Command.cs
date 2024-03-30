using System;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class Command : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        public float speedScroll;
        [field: SerializeField] public TypeCommand Type { get; private set; }
        [field: SerializeField] public ParentCommand Parent { get; private set; } = new();

        [field: SerializeField] public GameObject RootContentCommand { get; private set; }
        [field: SerializeField] public CommandManager CommandManager { get; private set; }
        [field: SerializeField] public CommandFunction CommandFunction { get; private set; }

        [SerializeField] private bool OnDrag = false;
        [SerializeField] private PointerEventData eventData;
        [SerializeField] private Vector3 beginDrag = new();
        [SerializeField] private Image image;
        [SerializeField] private Color DefaultColor;
        [SerializeField] private Color imageColor;
        [SerializeField] private ScrollRect scrollControl;
        [SerializeField] private VerticalLayoutGroup verticalLayout;
        [SerializeField] private ContentSizeFitter contentSize;

        void Awake()
        {
            RootContentCommand = GameObject.FindGameObjectWithTag(StaticText.RootListContentCommand);
            CommandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
            scrollControl = CommandManager.transform.Find("Scroll View").GetComponent<ScrollRect>();
            verticalLayout = RootContentCommand.GetComponent<VerticalLayoutGroup>();
            contentSize = RootContentCommand.GetComponent<ContentSizeFitter>();
        }

        void Start()
        {
            gameObject.tag = StaticText.TagCommand;
            CommandFunction = transform.GetChild(0).GetComponent<CommandFunction>();
            image = gameObject.GetComponent<Image>();
            Parent.UpdateParentAndIndex(transform.parent, transform.GetSiblingIndex());
            if (Type != TypeCommand.Null)
            {
                if (Type == TypeCommand.Behavior)
                {
                    gameObject.AddComponent<CommandBehavior>();
                    DefaultColor = image.color;
                }
                else
                {
                    if (transform.GetChild(0).GetComponent<CommandFunction>() == null)
                    {
                        gameObject.AddComponent<CommandFunction>();
                    }
                    else
                    {
                        DefaultColor = transform.GetChild(0).GetComponent<Image>().color;
                    }

                    verticalLayout.enabled = false;
                    verticalLayout.enabled = true;
                }
            }
        }

        void Update()
        {
            if (OnDrag)
            {
                float distance = eventData.pointerDrag.transform.position.y - beginDrag.y;
                float distanceConvert = (float)Math.Round(distance / 500f, 6);
                scrollControl.verticalNormalizedPosition += Time.deltaTime * distanceConvert;
                //print(distanceConvert);
            }
        }

        public void PlayingAction()
        {
            if (Type == TypeCommand.Behavior)
            {
                image.color = Color.black;
            }
            else
            {
                transform.GetChild(0).GetComponent<Image>().color = Color.black;
            }
        }

        public void ResetAction()
        {
            if (Type == TypeCommand.Behavior)
            {
                image.color = DefaultColor;
            }
            else
            {
                transform.GetChild(0).GetComponent<Image>().color = DefaultColor;
            }
        }

        public void UpdateType(TypeCommand typeCommand)
        {
            Type = typeCommand;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            //Debug.Log($"OnPointerDown '{gameObject.name}'");
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (!CommandManager.DataThisGame.playActionCommand)
            {
                //Debug.Log($"OnBeginDrag '{gameObject.name}'");
                this.eventData = eventData;
                Parent.UpdateParentAndIndex(transform.parent, transform.GetSiblingIndex());
                transform.SetParent(transform.root);
                transform.SetAsLastSibling();
                image.raycastTarget = false;
                imageColor = image.color;
                image.color = new Color(imageColor.r, imageColor.g, imageColor.b, 0.1f);
                GetComponentInChildren<Text>().raycastTarget = false;
                GetComponent<Button>().enabled = false;
                OnDrag = true;
                verticalLayout.enabled = false;
                contentSize.enabled = false;
                CommandManager.DropRemoveCommand.SetActive(OnDrag);
                beginDrag = scrollControl.transform.position;
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (!CommandManager.DataThisGame.playActionCommand)
            {
                //Debug.Log($"OnDrag '{gameObject.name}'");
                transform.position = Input.mousePosition;
                //print(RootContentCommand.transform.position.y - eventData.pointerDrag.transform.position.y);
            }
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (!CommandManager.DataThisGame.playActionCommand)
            {
                //Debug.Log($"OnEndDrag '{gameObject.name}'");
                transform.SetParent(Parent.parent);
                transform.SetSiblingIndex(Parent.index);
                foreach (Transform child in transform)
                {
                    child.gameObject.SetActive(true);
                }
                image.raycastTarget = true;
                image.color = imageColor;
                GetComponentInChildren<Text>().raycastTarget = true;
                GetComponent<Button>().enabled = true;
                OnDrag = false;
                verticalLayout.enabled = true;
                contentSize.enabled = true;
                CommandManager.DropRemoveCommand.SetActive(OnDrag);
                if (Type == TypeCommand.Function)
                {
                    CommandFunction command = transform.GetChild(0).GetComponent<CommandFunction>();
                    command.UpdateColor(command.RootContentCommand.transform);
                }
            }
        }

        void IDropHandler.OnDrop(PointerEventData eventData)
        {
            if (!CommandManager.DataThisGame.playActionCommand)
            {
                Command dropCommandObject = eventData.pointerDrag.GetComponent<Command>();

                if (Type == TypeCommand.Function)
                {
                    RectTransform targetRect = eventData.pointerEnter.GetComponent<RectTransform>();
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(targetRect, eventData.position, eventData.pressEventCamera, out Vector2 localPoint);

                    // Check if the dragged point is near the top or bottom edge
                    float topDistance = Mathf.Abs(localPoint.y - targetRect.rect.yMax);
                    float bottomDistance = Mathf.Abs(localPoint.y - targetRect.rect.yMin);
                    //print($"{topDistance} {bottomDistance}");
                    bool nearTopEdge = false;
                    bool nearBottomEdge = false;
                    if (topDistance < 40)
                    {
                        //Debug.Log("Near top edge");
                        dropCommandObject.Parent.UpdateIndex(transform.GetSiblingIndex());
                        dropCommandObject.Parent.UpdateParent(transform.parent);
                        nearTopEdge = true;
                    }

                    if (bottomDistance < 40)
                    {
                        //Debug.Log("Near bottom edge");
                        dropCommandObject.Parent.UpdateIndex(transform.GetSiblingIndex() + 1);
                        dropCommandObject.Parent.UpdateParent(transform.parent);
                        nearBottomEdge = true;
                    }
                    if ((nearTopEdge && nearBottomEdge) || (!nearTopEdge && !nearBottomEdge)) dropCommandObject.Parent.UpdateParentAndIndex(transform, 1);
                }
                else
                {
                    if (transform.position.y >= dropCommandObject.transform.position.y)
                    {
                        dropCommandObject.Parent.UpdateIndex(transform.GetSiblingIndex() + 1);
                    }
                    else
                    {
                        dropCommandObject.Parent.UpdateIndex(transform.GetSiblingIndex());
                    }
                    dropCommandObject.Parent.UpdateParent(transform.parent);
                }

                //Debug.Log($"'{dropCommandObject.name}' OnDrop '{gameObject.name}'");
            }
        }
    }
}