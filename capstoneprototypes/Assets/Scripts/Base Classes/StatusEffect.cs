using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0 - New StatEffect", menuName = "StatEffect/Create New StatEffect")]
public class StatusEffect : ScriptableObject
{
    [Header("Functional")]
    public int id;

    [Header("Flavor")]
    public string statusName;
    public string statusDescription;
}
