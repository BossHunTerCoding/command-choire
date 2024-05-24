using System;
using System.Collections.Generic;
using UnityEngine;

namespace CommandChoice.Model
{
    [Serializable]
    class FieldTransform
    {
        [field: SerializeField] public List<Transform> listTransform { get; private set; } = new List<Transform>();
    }
}