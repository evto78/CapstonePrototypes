using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0 - New Enemy Skill", menuName = "Skill/Create New Enemy Skill")]
public class EnemySkill : ScriptableObject
{
    [Header("Functional")]
    public int id;

    [Header("Flavor")]
    public string skillName;
    public string skillDescription;
}
