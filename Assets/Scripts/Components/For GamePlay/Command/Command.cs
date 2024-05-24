using System;
using CommandChoice.Data;
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

        public bool isLock = false;
        [SerializeField] private int indexLock = 0;
        [SerializeField] private GameObject LockObject;

        void Awake()
        {
            RootContentCommand = GameObject.FindGameObjectWithTag(StaticText.RootListContentCommand);
            CommandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
            scrollControl = CommandManager.transform.Find("Scroll View Command").GetComponent<ScrollRect>();
            verticalLayout = RootContentCommand.GetComponent<VerticalLayoutGroup>();
            contentSize = RootContentCommand.GetComponent<ContentSizeFitter>();
            gameObject.tag = StaticText.TagCommand;
        }

        void Start()
        {
            if (isLock)
            {
                LockCommand(transform);
            }
            CommandFunction = transform.GetChild(0).GetComponent<CommandFunction>();
            image = gameObject.GetComponent<Image>();
            Parent.UpdateParentAndIndex(transform.parent, transform.GetSiblingIndex());
            if (Type != TypeCommand.Null)
            {
                if (Type == TypeCommand.Behavior)
                {
                    if (gameObject.GetComponent<CommandBehavior>() == null) gameObject.AddComponent<CommandBehavior>();
                    DefaultColor = image.color;
                }
                else
                {
                    if (transform.GetChild(0).GetComponent<CommandFunction>() == null)
                    {
                        if (gameObject.GetComponent<CommandFunction>() == null) gameObject.AddComponent<CommandFunction>();
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
            if (isLock && transform.GetSiblingIndex() != (indexLock == 0 ? transform.GetSiblingIndex() : indexLock))
            {
                transform.SetSiblingIndex(indexLock == 0 ? transform.GetSiblingIndex() : indexLock);
            }
            if (OnDrag && !isLock)
            {
                float distance = eventData.pointerDrag.transform.position.y - beginDrag.y;
                float distanceConvert = (float)Math.Round(distance / 500f, 6);
                scrollControl.verticalNormalizedPosition += Time.deltaTime * distanceConvert;
                //print(distanceConvert);
            }
        }

        void OnDestroy()
        {
            DataGlobal.GamePlay.OnDragCommand = false;
        }

        void LockCommand(Transform transformParent){
            foreach (Transform item in transformParent)
            {
                print(item.name);
                if (item.gameObject.TryGetComponent(out Command command)) { command.isLock = true; }
                if (item.gameObject.TryGetComponent(out Image image)) { if (image.name == "lock") image.enabled = true; }
                if (item.gameObject.TryGetComponent(out Button button)) { button.enabled = false; }
                if (item.childCount > 0)
                {
                    LockCommand(item);
                }
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
            ScrollTo(gameObject.GetComponent<RectTransform>());
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

        public bool CheckSkipTo()
        {
            if (gameObject.TryGetComponent(out SkipToCommand skipToCommand))
            {
                return skipToCommand.activeSelect && !DataGlobal.GamePlay.playActionCommand;
            }
            return DataGlobal.GamePlay.CanDargCommand;
        }

        public void ScrollTo(RectTransform target)
        {
            // Find the scroll content RectTransform
            RectTransform contentRectTransform = scrollControl.content;

            // Calculate the normalized position
            Vector2 targetPosition = (Vector2)scrollControl.transform.InverseTransformPoint(contentRectTransform.position)
                                   - (Vector2)scrollControl.transform.InverseTransformPoint(target.position);

            // Adjust for pivot and anchor points
            Vector2 viewportSize = scrollControl.viewport.rect.size;
            Vector2 contentSize = contentRectTransform.rect.size;
            Vector2 pivotOffset = contentRectTransform.pivot * contentSize;
            Vector2 anchorOffset = (Vector2)scrollControl.viewport.anchorMin * viewportSize;

            // Calculate the normalized position
            Vector2 normalizedPosition = new Vector2(
                1f - (targetPosition.x / (pivotOffset.x - anchorOffset.x)),
                1f - (targetPosition.y / (pivotOffset.y - anchorOffset.y))
            );

            // Clamp the values between 0 and 1
            normalizedPosition.x = Mathf.Clamp01(normalizedPosition.x);
            normalizedPosition.y = Mathf.Clamp01(normalizedPosition.y);

            print(normalizedPosition);

            // Set the normalized position of the scroll rect
            scrollControl.normalizedPosition = normalizedPosition;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            //this.eventData = eventData;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (CheckSkipTo() && !isLock)
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
                DataGlobal.GamePlay.OnDragCommand = OnDrag;
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
            if (CheckSkipTo() && !isLock)
            {
                //Debug.Log($"OnDrag '{gameObject.name}'");
                transform.position = Input.mousePosition;
                //print(RootContentCommand.transform.position.y - eventData.pointerDrag.transform.position.y);
            }
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (CheckSkipTo() && !isLock)
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
                DataGlobal.GamePlay.OnDragCommand = OnDrag;
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
            if (CheckSkipTo() && !isLock)
            {
                if (!eventData.pointerDrag.TryGetComponent(out Command dropCommandObject)) return;

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
                    if (((nearTopEdge && nearBottomEdge) || (!nearTopEdge && !nearBottomEdge)) && gameObject.name != StaticText.Warp && gameObject.name != StaticText.SkipTo) dropCommandObject.Parent.UpdateParentAndIndex(transform, 1);
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