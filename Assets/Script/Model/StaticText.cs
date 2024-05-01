using System.Collections.Generic;
using CommandChoice.Component;

namespace CommandChoice.Model
{
    class StaticText
    {
        public const string MoveUp = "Move Up";
        public const string MoveDown = "Move Down";
        public const string MoveLeft = "Move Left";
        public const string MoveRight = "Move Right";
        public const string Idle = "Idle";
        public const string Break = "Break";
        public const string Count = "Count";
        public const string If = "If";
        public const string Else = "Else";
        public const string Loop = "Loop";
        public const string EndLoop = "End Loop";
        public const string SkipTo = "Skip To";
        public const string Trigger = "Trigger";
        public const string ObjectChild = "Object Child";
        public const string MailIcon = "Mail icon";
        public const string UiMail = "Ui Mail";
        public const string TagMail = "Mail";
        public const string TagEnemy = "Enemy";
        public const string TagHP = "HP";
        public const string TagCommand = "Command";
        public const string Untagged = "Untagged";
        public const string TagCanvas = "Canvas";
        public const string TagPlayer = "Player";
        public const string TagCamera = "MainCamera";
        public const string PathLevelScene = "Assets/Scene/Level";
        public const string PathImgMinimize = "Icon/minimize";
        public const string PathImgZoom = "Icon/zoom";
        public const string PathPrefabPauseGame = "Ui/Menu/PausePanels";
        public const string PathPrefabConfigCommandForFunction = "Ui/Command/Config Command Function";
        public const string PathPrefabBlockCommandForFunction = "Ui/Command/Block Command Function";
        public const string PathPrefabCommand = "Ui/Button/Command";
        public const string PathPrefabCommandSpecial = "Ui/Button/Command Special";
        public const string RootListContentCommand = "List Content Command";
        public const string RootListViewCommand = "List View Command";
        public const string RootFixCommand = "Fix Command";

        public static bool CheckCommand(string command)
        {
            if (CheckCommandBehavior(command)) return true;
            return CheckCommandFunction(command);
        }

        public static bool CheckCommandBehavior(string command)
        {
            List<string> listCommand = new() { MoveUp, MoveDown, MoveLeft, MoveRight, Idle };
            foreach (string item in listCommand)
            {
                if (command == item) return true;
            }

            return false;
        }

        public static bool CheckCommandFunction(string command)
        {
            List<string> listCommand = new() { Break, Count, If, Else, Loop, SkipTo, Trigger };
            foreach (string item in listCommand)
            {
                if (command == item) return true;
            }

            return false;
        }

        public static bool CheckCommandCanConfig(string command)
        {
            List<string> listCommand = new() { Loop };
            foreach (string item in listCommand)
            {
                if (command == item) return true;
            }

            return false;
        }

        public static bool CheckCommandCanTrigger(string command)
        {
            List<string> listCommand = new() { If, Else, SkipTo, Trigger };
            foreach (string item in listCommand)
            {
                if (command == item) return true;
            }

            return false;
        }

        public static string CommandDisplay(string nameCommand, CommandFunction commandFunction, string oldNameCommand = "")
        {
            string textUpdate = "";

            switch (nameCommand)
            {
                case Idle:
                    textUpdate = nameCommand;
                    break;
                case MoveUp:
                    textUpdate = nameCommand;
                    break;
                case MoveDown:
                    textUpdate = nameCommand;
                    break;
                case MoveLeft:
                    textUpdate = nameCommand;
                    break;
                case MoveRight:
                    textUpdate = nameCommand;
                    break;
                case Break:
                    textUpdate = nameCommand;
                    break;
                case Count:
                    textUpdate = $"{nameCommand} : {commandFunction.countTime}";
                    break;
                case If:
                    textUpdate = $"{nameCommand} : {commandFunction.trigger}";
                    break;
                case Else:
                    textUpdate = nameCommand;
                    break;
                case Loop:
                    textUpdate = $"{nameCommand} : Count {commandFunction.countTime}";
                    break;
                case SkipTo:
                    textUpdate = $"{nameCommand} : {commandFunction.trigger}";
                    break;
                case Trigger:
                    textUpdate = oldNameCommand;
                    break;
            }
            return textUpdate;
        }
    }
}