using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerObject : MonoBehaviour
{
    public enum markerType { EnemyAtk, PlayerAtk, SpeedUp, SpeedDown}
    public markerType type;

    public SpriteRenderer sr;
    public List<Sprite> markerSprites;

    public float timePos;
    
    public void SetType(markerType newType)
    {
        type = newType;

        sr.sprite = markerSprites[newType.GetHashCode()];
    }
    private void Update()
    {
        UpdateVisuals();
    }
    void UpdateVisuals()
    {

    }
}
