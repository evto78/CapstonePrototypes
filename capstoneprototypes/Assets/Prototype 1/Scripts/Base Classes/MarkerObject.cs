using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarkerObject : MonoBehaviour
{
    public SpriteRenderer sr;
    public List<Sprite> markerSprites;
    
    public void SetType(TimelineManager.EventType newType)
    {
        sr.sprite = markerSprites[newType.GetHashCode()];
    }
}
