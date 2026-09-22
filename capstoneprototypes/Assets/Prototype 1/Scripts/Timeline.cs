using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
    public GameObject markerPrefab;
    List<RectTransform> activeMarkers;
    public Vector2 minMax;
    public float combatTime;
    public float combatSpeed;
    float combatTimeLeft;
    public Image fillBar;
    public Transform thresholdBar;

    public bool speedUp;
    public float speedUpAmt;
    public float speedUpTime;

    public bool movePixelByPixel;
    private void Awake()
    {
        activeMarkers = new List<RectTransform>();
    }
    private void Start()
    {
        markerPrefab.SetActive(false);
        StartCombat();
    }

    public void StartCombat() 
    {
        enemyID = 0;
        combatTimeLeft = combatTime;
        fillBar.fillAmount = 0;
        combatSpeed = 1;
    }

    private void Update()
    {
        combatTimeLeft -= Time.deltaTime * combatSpeed;

        UpdateVisual();
    }
    void UpdateVisual()
    {
        float trueFillAmt = 1 - (combatTimeLeft / combatTime);
        if (movePixelByPixel)
        {
            float timelineRes = 160;
            //fillBar.fillAmount = Mathf.Round(trueFillAmt * timelineRes) / timelineRes;
        }
        else
        {
            //fillBar.fillAmount = trueFillAmt;
        }

        foreach (Transform activeMark in activeMarkers)
        {
            MarkerObject mark = activeMark.GetComponent<MarkerObject>();
            if (mark.timePos <= trueFillAmt) { mark.gameObject.SetActive(false); }
        }

        //thresholdBar.transform.localPosition = new Vector3(Mathf.Lerp(minMax.x, minMax.y, fillBar.fillAmount), 0, 0);
        //thresholdBar.gameObject.SetActive(thresholdBar.transform.localPosition.x > minMax.x + 3);
    }
    int enemyID = 0;
    public void AddEnemyAtkMarkers(float atkInterval, MarkerObject.markerType type)
    {
        float tempCounter = 0f;

        if (atkInterval <= 0) {Debug.Log("Interval is less than 0!!"); return; }

        while (tempCounter <= combatTime)
        {
            tempCounter += atkInterval;
            if (tempCounter <= combatTime)
            {
                AddMarker(type, tempCounter / combatTime, -enemyID);
            }
        }

        enemyID++;
    }
    public void AddMarker(MarkerObject.markerType type, float position, float yOffset)
    {
        RectTransform newMarker = Instantiate(markerPrefab, transform.GetChild(0)).GetComponent<RectTransform>();
        Debug.Log(newMarker);
        Debug.Log(activeMarkers);
        activeMarkers.Add(newMarker);
        MarkerObject markObj = newMarker.GetComponent<MarkerObject>();
        markObj.timePos = position;
        markObj.SetType(type);
        markObj.sr.sortingOrder += Mathf.CeilToInt(-yOffset);
        newMarker.transform.localPosition = new Vector3(Mathf.Lerp(minMax.x, minMax.y, position), yOffset, 0);
        newMarker.gameObject.SetActive(true);
    }
}
