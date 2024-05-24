using System;
using UnityEngine;

namespace CommandChoice.Model
{
    [Serializable]
    class GridModel
    {
        [field: SerializeField] public int Width { get; private set; } = 20;
        [field: SerializeField] public int Height { get; private set; } = 20;
        [field: SerializeField] public Color32 FirstColor { get; private set; } = new Color32(197, 197, 197, 255);
        [field: SerializeField] public Color32 SecondColor { get; private set; } = new Color32(152, 152, 152, 255);
    }
}