using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill/Create New Skill")]
public class PlayerSkill : ScriptableObject
{
    [Header("Functional")]
    public int id;
    public int cost;
    public enum TargetType { Self, SingleEnemy, AllEnemies }
    public TargetType target;

    [Header("Flavor")]
    public string skillName;
    public string skillDescription;

    //Can the skill be perfected with a timed input?
    public bool perfectable;
}
