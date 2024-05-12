using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
class FieldTransform
{
    [field: SerializeField] public List<Transform> listTransform { get; private set; } = new List<Transform>();
}