using System;
using UnityEngine;
using UnityEngine.Serialization;
using static UnityEngine.UI.Button;

namespace CommandChoice.Model
{
    public enum DialogPosition { Left, Center, Right, Default }

    [Serializable]
    public class DialogModel
    {
        public int indexDialog = 0;
        [field: SerializeField] public CharacterDetail[] CharacterDetail { get; private set; }
        [field: SerializeField] public DialogDetail[] DialogDetail { get; private set; }

        public (string, Sprite) GetCharacterDetail(int indexCharacter)
        {
            return (CharacterDetail[indexCharacter].NameCharacter, CharacterDetail[indexCharacter].ImageCharacter);
        }

        public Vector2 GetPositionCharacter(DialogPosition dialogPosition, Vector2 transformDialog)
        {
            switch (dialogPosition)
            {
                case DialogPosition.Left:
                    return new(-600, transformDialog.y);
                case DialogPosition.Center:
                    return new(0, transformDialog.y);
                case DialogPosition.Right:
                    return new(600, transformDialog.y);
                default: return transformDialog;
            }
        }

        public Vector2 GetPositionTextBox(DialogPosition dialogPosition, Vector2 transformDialog)
        {
            switch (dialogPosition)
            {
                case DialogPosition.Left:
                    return new(-700, transformDialog.y);
                case DialogPosition.Center:
                    return new(-550, transformDialog.y);
                case DialogPosition.Right:
                    return new(-400, transformDialog.y);
                default: return transformDialog;
            }
        }
    }

    [Serializable]
    public class DialogDetail
    {
        [field: SerializeField] public int ElementCharacter { get; private set; }
        [field: SerializeField] public bool ShowImageCharacter { get; private set; } = true;
        [field: SerializeField] public DialogPosition PositionCharacter { get; private set; } = DialogPosition.Left;
        [field: SerializeField] public string TextDialog { get; private set; }
        [field: SerializeField] public bool ShowBoxTextDialog { get; private set; } = true;
        [field: SerializeField] public DialogPosition PositionBoxTextDialog { get; private set; } = DialogPosition.Right;
        [field: Range(0, 255)]
        [field: SerializeField] public float dimBackground { get; private set; } = 80;
        [field: SerializeField] public Animator EventAnimation { get; private set; }
        [field: SerializeField] public string AnimationName { get; private set; }
        [field: SerializeField] public bool DestroyObjectAnimation { get; private set; } = false;
    }

    [Serializable]
    public class CharacterDetail
    {
        [field: SerializeField] public string NameCharacter { get; private set; }
        [field: SerializeField] public Sprite ImageCharacter { get; private set; }
    }
}
