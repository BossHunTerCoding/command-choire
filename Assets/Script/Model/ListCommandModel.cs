using System;
using System.Collections.Generic;
using System.Data;
using CommandChoice.Component;
using UnityEngine;

namespace CommandChoice.Model
{
    [Serializable]
    public class ListCommandModel
    {
        [SerializeField]
        public List<CommandModel> commandBehavior;

        [SerializeField]
        public List<CommandModel> commandFunctions;
        [SerializeField]
        public List<CommandModel> commandEnv;
        [SerializeField]
        public List<Color32> listColorCommands;

        public bool CommandIsEmpty(int command)
        {
            int countActive = 0;

            foreach (var item in command == 0 ? commandBehavior : commandFunctions)
            {
                if (item.Active) { countActive++; }
            }
            return countActive == 0;
        }

        public void ReturnCommand(Command command)
        {
            if (command.Type == TypeCommand.Behavior)
            {
                foreach (CommandModel item in commandBehavior)
                {
                    if (command.gameObject.name == item.Name) item.ReturnCommand();
                }
            }
            else
            {
                foreach (CommandModel item in commandFunctions)
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
                List<CommandModel> checkCommand = command.Type == TypeCommand.Behavior ? commandBehavior : commandFunctions;
                foreach (CommandModel item in checkCommand)
                {
                    if (command.gameObject.name == item.Name) item.ReturnCommand();
                }
                if (command.Type == TypeCommand.Function) LoopCheckChildCommandFunction(child.transform);
            }
        }

        ListCommandModel()
        {
            commandBehavior = new List<CommandModel>(){
                new(StaticText.MoveUp),
                new(StaticText.MoveDown),
                new(StaticText.MoveLeft),
                new(StaticText.MoveRight),
                new(StaticText.Idle),
            };

            commandFunctions = new List<CommandModel>(){
                new(StaticText.Break),
                new(StaticText.Count),
                new(StaticText.If),
                new(StaticText.Else),
                new(StaticText.Loop),
                new(StaticText.SkipTo),
                new(StaticText.Trigger),
            };

            commandEnv = new List<CommandModel>(){
                new("sand"),
                new("Dirt"),
            };

            listColorCommands = new List<Color32>(){
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

        [field: SerializeField] public int CanUse { get; private set; } = 0;
        [field: SerializeField] public bool InfinityCanUse { get; private set; } = false;

        public CommandModel(string NameCommand) { Name = NameCommand; }

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


