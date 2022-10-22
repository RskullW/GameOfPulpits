using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Teleport
{
    public string Name => In.name;
    public bool IsActive;
    public GameObject In;
    public GameObject Out;
}