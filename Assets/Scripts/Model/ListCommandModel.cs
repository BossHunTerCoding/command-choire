using System;
using System.Collections.Generic;
using CommandChoice.Component;
using UnityEngine;

namespace CommandChoice.Model
{
    [Serializable]
    public class ListCommandModel
    {
        [field: SerializeField] public List<CommandModel> CommandBehavior { get; private set; }
        [field: SerializeField] public List<CommandModel> CommandFunctions { get; private set; }
        [field: SerializeField] public List<CommandModel> CommandEnvironment { get; private set; }
        [field: SerializeField] public List<Color32> ListColorCommands { get; private set; }

        public void ReturnCommand(Command command)
        {
            if (command.Type == TypeCommand.Behavior)
            {
                foreach (CommandModel item in CommandBehavior)
                {
                    if (command.gameObject.name == item.Name) item.ReturnCommand();
                }
            }
            else
            {
                foreach (CommandModel item in CommandFunctions)
                {
                    if (command.gameObject.name == item.Name) item.ReturnCommand();
                }
                LoopCheckChildCommandFunction(command.transform);
            }
        }

        void LoopCheckChildCommandFunction(Transform transformCommand)
        {
            foreach (Transform child in transformCommand)
            {
                Command command = child.GetComponent<Command>();
                if (command == null) continue;
                List<CommandModel> checkCommand = command.Type == TypeCommand.Behavior ? CommandBehavior : CommandFunctions;
                foreach (CommandModel item in checkCommand)
                {
                    if (command.gameObject.name == item.Name) item.ReturnCommand();
                }
                if (command.Type == TypeCommand.Function) LoopCheckChildCommandFunction(child.transform);
            }
        }

        ListCommandModel()
        {
            CommandBehavior = new List<CommandModel>(){
                new(StaticText.MoveUp),
                new(StaticText.MoveDown),
                new(StaticText.MoveLeft),
                new(StaticText.MoveRight),
                new(StaticText.Idle),
            };

            CommandFunctions = new List<CommandModel>(){
                new(StaticText.Break),
                new(StaticText.Count),
                new(StaticText.If),
                new(StaticText.Else),
                new(StaticText.Loop),
                new(StaticText.SkipTo),
                new(StaticText.Trigger),
                new(StaticText.Warp),
            };

            CommandEnvironment = new List<CommandModel>() { };

            ListColorCommands = new List<Color32>(){
                new(244, 132, 132, 255),
                new(255, 0, 102, 255),
                new(255, 0, 0, 255),
                new(0, 176, 80, 255)
            };
        }
    }

    [Serializable]
    public class ParentCommand
    {
        public Transform parent;
        public int index = 0;

        public void UpdateParentAndIndex(Transform TransformParent, int IndexInParent)
        {
            parent = TransformParent;
            index = IndexInParent;
        }

        public void UpdateParent(Transform TransformParent) { parent = TransformParent; }

        public void UpdateIndex(int IndexInParent) { index = IndexInParent; }
    }

    [Serializable]
    public class CommandModel
    {
        [field: SerializeField] public bool Active { get; private set; } = false;

        [field: SerializeField] public string Name { get; private set; } = "Command";
        [field: SerializeField] public GameObject PreFabCommand { get; private set; }

        [field: SerializeField] public int CanUse { get; private set; } = 0;
        [field: SerializeField] public bool InfinityCanUse { get; private set; } = false;

        public CommandModel(string NameCommand, bool Active = false, bool Infinity = false, int CanUseCount = 0)
        {
            Name = NameCommand; this.Active = Active; InfinityCanUse = Infinity; CanUse = CanUseCount;
        }

        public void UsedCommand() { if (InfinityCanUse) return; CanUse -= 1; }

        public void ReturnCommand() { if (InfinityCanUse) return; CanUse += 1; }
    }

    public enum TypeCommand
    {
        Null,
        Behavior,
        Function
    }
}


