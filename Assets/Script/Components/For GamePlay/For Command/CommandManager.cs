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
        [field: SerializeField] public DataGamePlay DataThisGame { get; private set; }
        [field: SerializeField] public int TimeCount { get; private set; } = 0;
        [field: SerializeField] public ListCommandModel ListCommandModel { get; private set; }
        [field: SerializeField] public Transform CommandContent { get; private set; }
        [SerializeField] private Text countTime;
        [field: SerializeField] public GameObject AddCommandButton { get; private set; }
        [field: SerializeField] public GameObject DropRemoveCommand { get; private set; }
        [field: SerializeField] public List<Transform> ListCommandSelected { get; private set; } = new();
        Transform parentCommandBreak;

        public static string triggerEvent = null;

        void Start()
        {
            try
            {
                Text TitleLevel = GameObject.Find("Title Level").GetComponent<Text>();
                foreach (var item in DataGlobal.Scene.ListLevelScene)
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
            }
            catch (System.Exception) { }

            DataThisGame = new();
            countTime.text = "";
        }

        public void RemoveCommand(GameObject command)
        {
            Destroy(command);
        }

        public void AddNewCommand(GameObject command)
        {
            command.transform.SetParent(CommandContent);
        }

        public void PlayAction(List<Transform> listCommand)
        {
            if (DataThisGame.EnemyObjects.Count > 0)
            {
                foreach (GameObject item in DataThisGame.EnemyObjects)
                {
                    if (!item.TryGetComponent<DogComponent>(out var enemy)) continue;
                    enemy.StopFootWalkDog();
                }
            }
            ListCommandSelected.Clear();
            StartCoroutine(RunCommand(LoopCheckCommand(listCommand)));
            DataThisGame.playActionCommand = true;
            AddCommandButton.SetActive(false);
        }

        public void UpdateTime(int time = 1)
        {
            countTime.text = $"Count: {TimeCount += time}";
        }

        public List<Transform> LoopCheckCommand(List<Transform> transformObject, bool ignoreTagTrigger = true)
        {
            foreach (Transform parent in transformObject)
            {
                if (parent.tag == "Trigger" && ignoreTagTrigger) continue;
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
                    Transform newObject = Resources.Load<GameObject>(StaticText.PathPrefabCommand).transform;
                    newObject.name = StaticText.EndLoop;
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
                    int loopCount = command.GetComponent<Command>().CommandFunction.countDefault;
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

            // foreach (Transform item in Output)
            // {
            //     Debug.Log(item.name);
            // }

            return OutputRunCommand;
        }

        private void LoopCheckCommand(Transform transformObject)
        {
            if (StaticText.CheckCommand(transformObject.gameObject.name)) ListCommandSelected.Add(transformObject);
            if (transformObject.childCount > 1 && transformObject.gameObject.name != StaticText.If)
            {
                foreach (Transform child in transformObject)
                {
                    LoopCheckCommand(child);
                }
            }
            if (transformObject.gameObject.name == StaticText.Loop)
            {
                Transform newObject = Resources.Load<GameObject>(StaticText.PathPrefabCommand).transform;
                newObject.name = StaticText.EndLoop;
                ListCommandSelected.Add(newObject);
            };
        }

        public void IfCheckCommand(string nameCommand)
        {
            Transform rootIfCommand = GameObject.FindWithTag(StaticText.RootFixCommand).transform;
            if (rootIfCommand.childCount == 0) return;
            foreach (GameObject item in rootIfCommand)
            {
                if (nameCommand == item.GetComponent<IfCommand>().command.name)
                {

                }
            }
        }

        public void StopActionAll()
        {
            StopAllCoroutines();
        }

        public void ResetAction(bool stopCoroutinesOnly = false)
        {
            StopAllCoroutines();
            if (stopCoroutinesOnly) return;
            parentCommandBreak = null;
            countTime.text = "";
            List<GameObject> ListAllCommand = new() { };
            ListAllCommand.AddRange(GameObject.FindGameObjectsWithTag(StaticText.TagCommand));
            ListAllCommand.AddRange(GameObject.FindGameObjectsWithTag("Trigger"));
            foreach (GameObject item in ListAllCommand)
            {
                Command command1 = item.GetComponent<Command>();
                if (command1 == null) continue;
                command1.ResetAction();
                CommandFunction command2 = command1.CommandFunction;
                if (command2 == null) continue;
                command2.countIf = false;
                command2.activeSkipTo = false;
                command2.UpdateTextCommand(item.name, true);
            }
            GameObject.FindGameObjectWithTag(StaticText.TagPlayer).GetComponent<PlayerManager>().ResetGame();
            if (DataThisGame.MailObjects.Count > 0)
            {
                foreach (GameObject item in DataThisGame.MailObjects)
                {
                    MailComponent mail = item.GetComponent<MailComponent>();
                    if (mail == null) continue;
                    mail.ResetGame();
                }
            }
            if (DataThisGame.EnemyObjects.Count > 0)
            {
                foreach (GameObject item in DataThisGame.EnemyObjects)
                {
                    DogComponent enemy = item.GetComponent<DogComponent>();
                    if (enemy == null) continue;
                    enemy.ResetGame();
                }
            }
            foreach (Transform item in GameObject.Find("Right-Bottom").transform)
            {
                if (item.gameObject.name == "Run") item.gameObject.SetActive(true);
                if (item.gameObject.name == "Reset") item.gameObject.SetActive(false);
            }
            DataThisGame.waitCoolDownJump = false;
            DataThisGame.playActionCommand = false;
            AddCommandButton.SetActive(true);
        }

        public IEnumerator RunCommand(List<Transform> listCommand, int? timeSet = null, bool IsChild = false)
        {
            if (timeSet == null) TimeCount = 0;
            else TimeCount = (int)timeSet;

            countTime.text = $"Count: {TimeCount}";
            PlayerManager player = GameObject.FindGameObjectWithTag(StaticText.TagPlayer).GetComponent<PlayerManager>();
            foreach (var item in listCommand.Select((value, index) => new { value, index }))
            {
                if (triggerEvent != null)
                {
                    GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Trigger");
                    string trigger = triggerEvent;
                    triggerEvent = null;
                    if (gameObjects.Length > 0)
                    {
                        foreach (GameObject gameObject in gameObjects)
                        {
                            if (gameObject.GetComponent<TriggerComponent>().tagTrigger == trigger)
                            {
                                ListCommandSelected.Clear();
                                List<Transform> transformsCommand = new() { gameObject.transform };
                                yield return RunCommand(LoopCheckCommand(transformsCommand, false), TimeCount);
                            }
                            else continue;
                        }
                    };
                }
                if (parentCommandBreak != null)
                {
                    if ((item.value.parent == parentCommandBreak) || (item.value == parentCommandBreak))
                    {
                        CommandFunction checkCommandLoop = parentCommandBreak.GetComponent<Command>().CommandFunction;
                        checkCommandLoop.countTime = checkCommandLoop.countDefault;
                        checkCommandLoop.UpdateTextCommand(parentCommandBreak.name);
                        continue;
                    }
                    else
                    {
                        parentCommandBreak = null;
                    }
                };
                yield return new WaitForSeconds(DataGlobal.timeDeray / float.Parse(GameObject.Find("Speed").transform.GetChild(0).GetComponent<Text>().text));
                if (item.value.name == StaticText.Loop)
                {
                    Command command = item.value.GetComponent<Command>();
                    if (!command.CommandFunction.countIf) countTime.text = $"Count: {++TimeCount}";
                    if (!command.CommandFunction.countIf) item.value.GetComponent<Command>().CommandFunction.countIf = true;
                }
                else if (item.value.name == StaticText.SkipTo)
                {
                    if (item.value.GetComponent<Command>().CommandFunction.activeSkipTo) continue;
                }
                else
                {
                    countTime.text = $"Count: {++TimeCount}";
                }
                try
                {
                    listCommand[item.index - 1].GetComponent<Command>().ResetAction();
                    listCommand[item.index].GetComponent<Command>().PlayingAction();
                }
                catch (System.Exception)
                {
                    listCommand[item.index].GetComponent<Command>().PlayingAction();
                }
                if (item.value.name == StaticText.MoveUp)
                {
                    yield return player.PlayerMoveUp(item.value.name);
                }
                else if (item.value.name == StaticText.MoveDown)
                {
                    yield return player.PlayerMoveDown(item.value.name);
                }
                else if (item.value.name == StaticText.MoveLeft)
                {
                    yield return player.PlayerMoveLeft(item.value.name);
                }
                else if (item.value.name == StaticText.MoveRight)
                {
                    yield return player.PlayerMoveRight(item.value.name);
                }
                else if (item.value.name == StaticText.Idle)
                {
                    yield return player.PlayerIdle(item.value.name);
                }
                else if (item.value.name == StaticText.Loop)
                {
                    Command command = item.value.GetComponent<Command>();
                    if (item.value.childCount > 1)
                    {
                        CheckLoopForCountTextUI(item.value);
                    }
                    command.CommandFunction.UsedLoopCount();
                    command.CommandFunction.UpdateTextCommand(item.value.name);
                }
                else if (item.value.name == StaticText.If)
                {
                    bool active = false;
                    print(item.value.transform.parent.transform.parent);
                    for (int i = 0; i < item.value.transform.parent.childCount; i++)
                    {
                        print(i);
                        if (i == item.value.GetSiblingIndex()) break;
                        if (item.value.transform.parent.GetChild(i).name == item.value.GetChild(0).GetComponent<IfCommand>().command.name)
                        {
                            active = true;
                            break;
                        }
                    }
                    print(active);
                    if (active)
                    {
                        ListCommandSelected.Clear();
                        List<Transform> transformsCommandIf = new();
                        for (int i = 0; i < item.value.childCount; i++)
                        {
                            if (i == 0) continue;
                            transformsCommandIf.Add(item.value.GetChild(i).transform);
                        }
                        yield return RunCommand(LoopCheckCommand(transformsCommandIf), TimeCount);
                    }
                    else continue;
                }
                else if (item.value.name == StaticText.Warp)
                {
                    if (player.Collider2D != null)
                    {
                        PortalWarpComponent portalWarp = player.Collider2D.GetComponent<PortalWarpComponent>();
                        if (portalWarp != null)
                        {
                            player.transform.position = new(portalWarp.LinkPortal.transform.position.x, portalWarp.LinkPortal.transform.position.y, player.transform.position.z);
                        }
                    }
                }
                else if (item.value.name == StaticText.SkipTo)
                {
                    SkipToCommand skipToCommand = item.value.GetComponent<SkipToCommand>();
                    if (!item.value.GetComponent<Command>().CommandFunction.activeSkipTo)
                    {
                        item.value.GetComponent<Command>().CommandFunction.activeSkipTo = true;
                        List<Transform> listTransform = new();
                        for (int i = skipToCommand.transformSelectCommand.GetSiblingIndex(); i < skipToCommand.transformSelectCommand.parent.transform.childCount; i++)
                        {
                            if (skipToCommand.transformSelectCommand.parent.transform.GetChild(i) == item.value) break;
                            listTransform.Add(skipToCommand.transformSelectCommand.parent.transform.GetChild(i));
                        }
                        yield return new WaitForSeconds(DataGlobal.timeDeray / float.Parse(GameObject.Find("Speed").transform.GetChild(0).GetComponent<Text>().text));
                        listCommand[item.index].GetComponent<Command>().ResetAction();
                        ListCommandSelected.Clear();
                        CheckLoopForCountTextUI(listTransform);
                        yield return RunCommand(LoopCheckCommand(listTransform), TimeCount);
                    }
                }
                yield return new WaitForSeconds(DataGlobal.timeDeray / float.Parse(GameObject.Find("Speed").transform.GetChild(0).GetComponent<Text>().text));
                listCommand[item.index].GetComponent<Command>().ResetAction();
                try
                {
                    if (player.Collider2D.GetComponent<MudPitComponent>() != null)
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
                    GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Trigger");
                    string trigger = triggerEvent;
                    triggerEvent = null;
                    if (gameObjects.Length > 0)
                    {
                        foreach (GameObject gameObject in gameObjects)
                        {
                            if (gameObject.GetComponent<TriggerComponent>().tagTrigger == trigger)
                            {
                                ListCommandSelected.Clear();
                                List<Transform> transformsCommand = new() { gameObject.transform };
                                yield return RunCommand(LoopCheckCommand(transformsCommand, false), TimeCount);
                            }
                            else continue;
                        }
                    };
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
                    item.GetComponent<Command>().CommandFunction.activeSkipTo = false;
                    item.GetComponent<Command>().CommandFunction.countIf = false;
                    if (item.GetComponent<Command>().CommandFunction.countTime <= 0)
                    {
                        CommandFunction checkCommandLoop = item.GetComponent<Command>().CommandFunction;
                        checkCommandLoop.countTime = checkCommandLoop.countDefault;
                        checkCommandLoop.UpdateTextCommand(item.name);
                    }
                    if (item.transform.childCount > 1)
                    {
                        CheckLoopForCountTextUI(item.transform);
                    }
                }
            }
        }

        void CheckLoopForCountTextUI(List<Transform> transformParent)
        {
            foreach (Transform item in transformParent)
            {
                if (item.name == StaticText.Loop)
                {
                    item.GetComponent<Command>().CommandFunction.activeSkipTo = false;
                    item.GetComponent<Command>().CommandFunction.countIf = false;
                    if (item.GetComponent<Command>().CommandFunction.countTime <= 0)
                    {
                        CommandFunction checkCommandLoop = item.GetComponent<Command>().CommandFunction;
                        checkCommandLoop.countTime = checkCommandLoop.countDefault;
                        checkCommandLoop.UpdateTextCommand(item.name);
                    }
                    if (item.transform.childCount > 1)
                    {
                        CheckLoopForCountTextUI(item.transform);
                    }
                }
            }
        }

        public void ConfigCommand(Command command, CommandFunction commandFunction)
        {
            if (command.Type != TypeCommand.Function) return;
            GameObject configPanels = Instantiate(Resources.Load<GameObject>(StaticText.PathPrefabConfigCommandForFunction), transform.root);
            ConfigCommand config = configPanels.GetComponent<ConfigCommand>();
            config.GetCommand(command, commandFunction);
        }

        public static void TriggerObjects(string tagObjectTrigger)
        {
            triggerEvent = tagObjectTrigger;
        }

        public static void SkipToConfig(string tagObjectTrigger)
        {
            triggerEvent = tagObjectTrigger;
        }
    }
}