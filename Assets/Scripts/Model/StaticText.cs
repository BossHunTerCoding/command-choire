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
        public const string Warp = "Warp";
        public const string SkipTo = "Skip To";
        public const string Trigger = "Trigger";
        public const string ObjectChild = "Object Child";
        public const string MailIcon = "Mail icon";
        public const string UiMail = "Ui Mail";
        public const string TagMail = "Mail";
        public const string TagEnemy = "Enemy";
        public const string TagTrough = "Trough";
        public const string TagMudPit = "MudPit";
        public const string TagPortal = "Portal";
        public const string TagPostalBox = "Postal Box";
        public const string TagHP = "HP";
        public const string TagCommand = "Command";
        public const string Untagged = "Untagged";
        public const string TagCanvas = "Canvas";
        public const string TagPlayer = "Player";
        public const string TagMusicBackGround = "MusicBackGround";
        public const string TagCamera = "MainCamera";
        public const string LayerWall = "Wall";
        public const string PathLevelScene = "Assets/Scene/Level";
        public const string PathImgMinimize = "Icon/minimize";
        public const string PathImgZoom = "Icon/zoom";
        public const string PathPrefabPauseGame = "Ui/Menu/PausePanels";
        public const string PathPrefabConfigCommandForFunction = "Ui/Command/Config Command Function";
        public const string PathPrefabBlockCommandForFunction = "Ui/Command/Block Command Function";
        public const string PathPrefabMenuListCommand = "Ui/Command/Menu List Commands";
        public const string PathPrefabCommand = "Ui/Button/Command";
        public const string PathPrefabCommandSpecial = "Ui/Button/Command Special";
        public const string RootListContentCommand = "List Content Command";
        public const string RootListViewCommand = "List View Command";
        public const string RootFixCommand = "Fix Command";

        public static bool CheckCommand(string command)
        {
            return CheckCommandFunction(command) || CheckCommandBehavior(command);
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
            List<string> listCommand = new() { Break, Count, If, Else, Loop, SkipTo, Trigger, Warp };
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

        public static string CommandDisplay(Command command)
        {
            string textUpdate = "";

            switch (command.name)
            {
                case Idle:
                    textUpdate = command.name;
                    break;
                case MoveUp:
                    textUpdate = command.name;
                    break;
                case MoveDown:
                    textUpdate = command.name;
                    break;
                case MoveLeft:
                    textUpdate = command.name;
                    break;
                case MoveRight:
                    textUpdate = command.name;
                    break;
                case Break:
                    textUpdate = command.name;
                    break;
                case Count:
                    textUpdate = command.name;
                    break;
                case If:
                    textUpdate = command.transform.GetChild(0).GetComponent<IfCommand>().command.name;
                    break;
                case Else:
                    textUpdate = command.name;
                    break;
                case Loop:
                    textUpdate = $"{command.name} : Count {command.GetComponent<LoopCommand>().countTime}";
                    break;
                case SkipTo:
                    textUpdate = command.name;
                    break;
                case Trigger:
                    textUpdate = command.name;
                    break;
                case Warp:
                    textUpdate = command.name;
                    break;
            }
            return textUpdate;
        }
    }
}