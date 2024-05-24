using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class CommandManager : MonoBehaviour
    {
        [SerializeField] bool debugLog = false;
        [SerializeField] private DataGamePlay DataThisGame;
        [field: SerializeField] public ListCommandModel ListCommandModel { get; private set; }
        [SerializeField] private Text textCommandCount;
        [SerializeField] private Text textUsedCommandCount;
        [SerializeField] private Transform CommandContent;
        [SerializeField] private GameObject AddCommandButton;
        [field: SerializeField] public GameObject DropRemoveCommand { get; private set; }
        [field: SerializeField] public GameObject ActionGameButton { get; private set; }
        [field: SerializeField] public GameObject JumpButton { get; private set; }
        [field: SerializeField] public List<Transform> ListCommandSelected { get; private set; } = new();
        [SerializeField] List<Transform> RunCommand;
        Transform parentCommandBreak;
        public static string triggerEvent = null;

        void Awake()
        {
            DataGlobal.ImportLevelScene();
            AddCommandButton.SetActive(false);
        }

        void Start()
        {
            Text TitleLevel = GameObject.Find("Title Level").GetComponent<Text>();
            foreach (var item in DataGlobal.LevelScene.ListLevelScene)
            {
                if (SceneManager.GetActiveScene().name == item.getNameForLoadScene())
                {
                    TitleLevel.text = item.NameLevelScene;
                    break;
                }
                else
                {
                    TitleLevel.text = "Level";
                }
            }
            textCommandCount.text = DataGlobal.GamePlay.commandCountTime.ToString();
            ActionGameButton = GameObject.Find("Right-Bottom");
            JumpButton = GameObject.Find("Jump");
        }

        void Update()
        {
            DataGlobal.GamePlay.time += Time.deltaTime;
            DataThisGame = DataGlobal.GamePlay;
            textUsedCommandCount.text = DataGlobal.GamePlay.commandUsedCount.ToString();
            textCommandCount.gameObject.SetActive(DataGlobal.GamePlay.playActionCommand);
            textCommandCount.text = $"Command: {DataGlobal.GamePlay.commandCountTime}";
            //AddCommandButton.SetActive(DataGlobal.GamePlay.CanClickButton);
            ActionGameButton.SetActive(!DataGlobal.GamePlay.OnDragCommand && !DataGlobal.GamePlay.activeSelectSkipToMode);
            JumpButton.SetActive(!DataGlobal.GamePlay.OnDragCommand && !DataGlobal.GamePlay.activeSelectSkipToMode);
            if (debugLog)
            {
                print(triggerEvent);
            }
        }

        public void AddNewCommand(GameObject command)
        {
            command.transform.SetParent(CommandContent);
        }

        public void StopActionAll()
        {
            StopAllCoroutines();
        }

        public void InitPlayAction(List<Transform> listCommand)
        {
            foreach (DogComponent enemy in DataGlobal.GamePlay.EnemyObjects)
            {
                enemy.StopFootWalk();
            }
            AddCommandButton.SetActive(false);
            ListCommandSelected.Clear();
            DataGlobal.GamePlay.playActionCommand = true;
            StartCoroutine(PlayAction(LoopCheckCommand(listCommand)));
        }

        public List<Transform> LoopCheckCommand(List<Transform> transformObject, bool ignoreTagTrigger = true)
        {
            foreach (Transform parent in transformObject)
            {
                if (parent.CompareTag(StaticText.Trigger) && ignoreTagTrigger) continue;
                if (StaticText.CheckCommand(parent.gameObject.name)) ListCommandSelected.Add(parent);
                if (parent.childCount > 1 && parent.gameObject.name != StaticText.If)
                {
                    foreach (Transform child in parent)
                    {
                        LoopCheckCommand(child);
                    }
                }
                if (parent.gameObject.name == StaticText.Loop)
                {
                    Transform newObject = new GameObject(StaticText.EndLoop).transform;
                    ListCommandSelected.Add(newObject);
                };
            }
            List<Transform> OutputRunCommand = new();

            Stack<int> loopIndexStack = new(); // Store index of "for" loops
            Stack<int> loopCountStack = new(); // Store loop count of "for" loops

            for (int i = 0; i < ListCommandSelected.Count; i++)
            {
                Transform command = ListCommandSelected[i];

                if (command.name.ToLower().StartsWith("loop"))
                {
                    int loopCount = command.GetComponent<LoopCommand>().countDefault;
                    loopIndexStack.Push(OutputRunCommand.Count); // Store current index of "for" loop
                    loopCountStack.Push(loopCount); // Store loop count of "for" loop
                    OutputRunCommand.Add(command); // Add "for" command to output
                }
                else if (command.name.ToLower().StartsWith("end"))
                {
                    int loopIndex = loopIndexStack.Pop(); // Retrieve index of corresponding "end" for "for" loop
                    int loopCount = loopCountStack.Pop(); // Retrieve loop count of "for" loop
                    List<Transform> loopContent = new(); // Create list to store content of "for" loop
                                                         // Retrieve content of "for" loop from output
                    for (int j = loopIndex; j < OutputRunCommand.Count; j++)
                    {
                        loopContent.Add(OutputRunCommand[j]);
                    }
                    // Add content of "for" loop to output according to loop count
                    for (int k = 1; k < loopCount; k++)
                    {
                        OutputRunCommand.AddRange(loopContent);
                    }
                }
                else
                {
                    OutputRunCommand.Add(command); // Add move command to output
                }
            }

            RunCommand.AddRange(OutputRunCommand);

            return OutputRunCommand;
        }

        private void LoopCheckCommand(Transform transformObject)
        {
            if (StaticText.CheckCommand(transformObject.name)) ListCommandSelected.Add(transformObject);
            if (transformObject.childCount > 1 && transformObject.gameObject.name != StaticText.If)
            {
                foreach (Transform child in transformObject)
                {
                    LoopCheckCommand(child);
                }
            }
            if (transformObject.gameObject.name == StaticText.Loop)
            {
                Transform newObject = new GameObject(StaticText.EndLoop).transform;
                ListCommandSelected.Add(newObject);
            };
        }

        public void ResetAction(bool stopCoroutinesOnly = false)
        {
            DataGlobal.GamePlay.countReply++;
            StopAllCoroutines();
            if (stopCoroutinesOnly) return;
            List<GameObject> ListAllCommand = new() { };
            ListAllCommand.AddRange(GameObject.FindGameObjectsWithTag(StaticText.TagCommand));
            ListAllCommand.AddRange(GameObject.FindGameObjectsWithTag("Trigger"));
            foreach (GameObject item in ListAllCommand)
            {
                if (item.TryGetComponent(out LoopCommand loop)) { loop.Reset(); }
                if (item.TryGetComponent(out SkipToCommand skip)) { skip.activeSkipTo = false; }
                if (item.TryGetComponent(out Command command)) { command.ResetAction(); }
            }
            GameObject.FindGameObjectWithTag(StaticText.TagPlayer).GetComponent<PlayerManager>().ResetGame();
            for (int i = 0; i < DataGlobal.GamePlay.MailObjects.Count; i++)
            {
                if (DataGlobal.GamePlay.MailObjects[i] == null) DataGlobal.GamePlay.MailObjects.RemoveAt(i);
                else DataGlobal.GamePlay.MailObjects[i].gameObject.SetActive(true);
            }
            foreach (DogComponent enemyObject in DataGlobal.GamePlay.EnemyObjects)
            {
                enemyObject.ResetGame();
            }
            foreach (TroughComponent Trough in DataGlobal.GamePlay.TroughObjects)
            {
                Trough.Reset();
            }
            foreach (Transform item in ActionGameButton.transform)
            {
                if (item.gameObject.name == "Run") item.gameObject.SetActive(true);
                if (item.gameObject.name == "Reset") item.gameObject.SetActive(false);
            }
            DataGlobal.GamePlay.ResetDefault();
            parentCommandBreak = null;
            triggerEvent = null;
            PlayerManager.Collider2D = null;
            ListCommandSelected.Clear();
            RunCommand.Clear();
        }

        public IEnumerator PlayAction(List<Transform> listCommand, bool IsChild = false)
        {
            PlayerManager player = GameObject.FindGameObjectWithTag(StaticText.TagPlayer).GetComponent<PlayerManager>();
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Trigger");
            foreach (var item in listCommand.Select((value, index) => new { value, index }))
            {
                ListCommandSelected.Clear();
                if (triggerEvent != null)
                {
                    string trigger = triggerEvent;
                    triggerEvent = null;
                    foreach (GameObject gameObject in gameObjects)
                    {
                        if (gameObject.GetComponent<TriggerComponent>().tagTrigger == trigger)
                        {
                            List<Transform> transformsCommand = new() { gameObject.transform };
                            yield return PlayAction(LoopCheckCommand(transformsCommand, false));
                        }
                        else continue;
                    }
                }
                if (parentCommandBreak != null)
                {
                    if ((item.value.parent == parentCommandBreak) || (item.value == parentCommandBreak))
                    {
                        LoopCommand checkCommandLoop = parentCommandBreak.GetComponent<LoopCommand>();
                        checkCommandLoop.countTime = checkCommandLoop.countDefault;
                        continue;
                    }
                    else
                    {
                        parentCommandBreak = null;
                    }
                };

                if (item.value.name == StaticText.Loop)
                {
                    LoopCommand command = item.value.GetComponent<LoopCommand>();
                    if (!command.countActive)
                    {
                        DataGlobal.GamePlay.UpdateTime();
                        command.countActive = true;
                        listCommand[item.index].GetComponent<Command>().PlayingAction();
                    }
                }
                else if (item.value.name == StaticText.SkipTo)
                {
                    if (!item.value.GetComponent<SkipToCommand>().activeSkipTo)
                    {
                        DataGlobal.GamePlay.UpdateTime();
                        listCommand[item.index].GetComponent<Command>().PlayingAction();
                        yield return new WaitForSeconds(DataGlobal.timeDeray / float.Parse(GameObject.Find("Speed").transform.GetChild(0).GetComponent<Text>().text));
                    }
                }
                else
                {
                    DataGlobal.GamePlay.UpdateTime();
                    listCommand[item.index].GetComponent<Command>().PlayingAction();
                    yield return new WaitForSeconds(DataGlobal.timeDeray / float.Parse(GameObject.Find("Speed").transform.GetChild(0).GetComponent<Text>().text));
                }

                if (item.value.name == StaticText.MoveUp)
                {
                    yield return player.PlayerMove(new Vector2(0f, 1f));
                }
                else if (item.value.name == StaticText.MoveDown)
                {
                    yield return player.PlayerMove(new Vector2(0f, -1f));
                }
                else if (item.value.name == StaticText.MoveLeft)
                {
                    yield return player.PlayerMove(new Vector2(-1f, 0f));
                }
                else if (item.value.name == StaticText.MoveRight)
                {
                    yield return player.PlayerMove(new Vector2(1f, 0f));
                }
                else if (item.value.name == StaticText.Idle)
                {
                    yield return player.PlayerMove(new Vector2(0f, 0f));
                }
                else if (item.value.name == StaticText.Loop)
                {
                    LoopCommand command = item.value.GetComponent<LoopCommand>();
                    if (item.value.childCount > 1)
                    {
                        CheckLoopForCountTextUI(item.value);
                    }
                    command.UsedLoopCount();
                }
                else if (item.value.name == StaticText.If)
                {
                    if (item.value.GetChild(0).GetComponent<IfCommand>().CheckIfCommand())
                    {
                        List<Transform> transformsCommandIf = new();
                        for (int i = 0; i < item.value.childCount; i++)
                        {
                            if (i == 0) continue;
                            transformsCommandIf.Add(item.value.GetChild(i).transform);
                        }
                        yield return PlayAction(LoopCheckCommand(transformsCommandIf));
                    }
                }
                else if (item.value.name == StaticText.Warp)
                {
                    if (PlayerManager.Collider2D.TryGetComponent(out PortalWarpComponent portalWarp))
                    {
                        player.transform.position = new(portalWarp.LinkPortal.transform.position.x, portalWarp.LinkPortal.transform.position.y, player.transform.position.z);
                    }
                }
                else if (item.value.name == StaticText.SkipTo)
                {
                    SkipToCommand skipToCommand = item.value.GetComponent<SkipToCommand>();
                    if (!skipToCommand.activeSkipTo)
                    {
                        skipToCommand.activeSkipTo = true;
                        yield return StartCoroutine(PlayAction(LoopCheckCommand(skipToCommand.PlaySkip())));
                    }
                }
                yield return new WaitForSeconds(DataGlobal.timeDeray / float.Parse(GameObject.Find("Speed").transform.GetChild(0).GetComponent<Text>().text));
                listCommand[item.index].GetComponent<Command>().ResetAction();
                try
                {
                    if (PlayerManager.Collider2D.TryGetComponent(out MudPitComponent value))
                    {
                        if (item.value.parent.name == StaticText.Loop)
                        {
                            parentCommandBreak = item.value.parent;
                        }
                    };
                }
                catch (System.Exception) { }
                if (triggerEvent != null)
                {
                    string trigger = triggerEvent;
                    triggerEvent = null;
                    foreach (GameObject gameObject in gameObjects)
                    {
                        if (gameObject.GetComponent<TriggerComponent>().tagTrigger == trigger)
                        {
                            List<Transform> transformsCommand = new() { gameObject.transform };
                            yield return PlayAction(LoopCheckCommand(transformsCommand, false));
                        }
                        else continue;
                    }
                }
            }
            //print("End Run");
        }

        void CheckLoopForCountTextUI(Transform transformParent)
        {
            foreach (Transform item in transformParent)
            {
                if (item.name == StaticText.Loop)
                {
                    item.GetComponent<LoopCommand>().countActive = false;
                    if (item.GetComponent<LoopCommand>().countTime <= 0)
                    {
                        LoopCommand checkCommandLoop = item.GetComponent<LoopCommand>();
                        checkCommandLoop.countTime = checkCommandLoop.countDefault;
                    }
                    if (item.transform.childCount > 1)
                    {
                        CheckLoopForCountTextUI(item.transform);
                    }
                }
                // if (item.name == StaticText.SkipTo)
                // {
                //     item.GetComponent<SkipToCommand>().activeSkipTo = false;
                // }
            }
        }

        public void CheckLoopForCountTextUI(List<Transform> transformParent)
        {
            foreach (Transform item in transformParent)
            {
                if (item.name == StaticText.Loop)
                {
                    //if (item.TryGetComponent(out SkipToCommand skip)) { skip.activeSkipTo = false; }
                    if (item.TryGetComponent(out LoopCommand loop)) { loop.Reset(); }
                    if (item.transform.childCount > 1)
                    {
                        CheckLoopForCountTextUI(item.transform);
                    }
                }
            }
        }
    }
}