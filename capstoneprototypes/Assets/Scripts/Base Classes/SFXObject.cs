using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "0 - New SFXObject", menuName = "Audio/Create New SFXObject")]
public class SFXObject : ScriptableObject
{
    [Header("Functional")]
    public int id;
    public AudioClip clip;
    public AudioManager.SFXType clipType;
}
