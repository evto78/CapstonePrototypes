using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Enemy/Create New Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Functional")]
    public int id;

    [Header("Flavor")]
    public string enemyName;
    public string enemyDescription;
}
