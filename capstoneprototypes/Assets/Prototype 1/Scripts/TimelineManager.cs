using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineManager : MonoBehaviour
{
    [System.Serializable]
    public class TimelineEvent
    {
        public EventType eventType;
        public int timelineIndex;
        public bool deleteOnActivate;
        public bool eventRun = false;
        public RectTransform markerTrans;
    }
    [System.Serializable]
    public enum EventType { EnemyAtk, PlayerAtk, SpeedUp, SpeedDown, Delay, ReverseStart, ReverseEnd, PortalStart, PortalEnd}
    public List<TimelineEvent> eventList;
    public float combatTime;
    public float timePassed;
    public float prevTimePassed;
    int prevEventIndex;
    public int eventIndex;
    public int roundNumber;
    public float combatSpeed;
    public bool combatActive;
    public float resolution;
    public List<Enemy> activeEnemies;
    [Header("Visuals")]
    public Image fillBar;
    public GameObject markerPrefab;
    public Vector2 minMaxPos;
    public Transform thresholdBar;
    public bool fillPixelByPixel;

    private void Awake()
    {
        activeEnemies = new List<Enemy>();
    }

    private void Start()
    {
        combatActive = false;
        markerPrefab.SetActive(false);

        StartCombat();
    }
    private void Update()
    {
        if (combatActive) { UpdateCombat(); }
        else { UpdateVisuals(); }
    }
    void UpdateVisuals()
    {
        if (fillPixelByPixel)
        {
            fillBar.fillAmount = eventIndex / resolution;
        }
        else
        {
            fillBar.fillAmount = timePassed / combatTime;
        }

        thresholdBar.transform.localPosition = new Vector3(Mathf.Lerp(minMaxPos.x, minMaxPos.y, fillBar.fillAmount), 0, 0);
    }
    public void AddEnemyAtkMarkers(float atkInterval, EventType type)
    {
        float tempCounter = 0f;

        if (atkInterval <= 0) { Debug.Log("Interval is less than 0!!"); return; }

        while (tempCounter <= combatTime)
        {
            tempCounter += atkInterval;
            if (tempCounter <= combatTime)
            {
                AddMarker(Mathf.FloorToInt((tempCounter / combatTime) * resolution), type, false);
            }
        }
    }
    public void StartCombat()
    {
        if (combatActive) { roundNumber++; } else { roundNumber = 0; }
        eventList = new List<TimelineEvent>();
        combatActive = true;
        timePassed = 0;
        prevEventIndex = -1;
        combatSpeed = 1;
    }
    public void EndCombat()
    {
        combatActive = false;
        timePassed = 0;
        roundNumber = 0;
        prevEventIndex = -1;
        combatSpeed = 0;
    }
    void UpdateCombat()
    {
        eventIndex = Mathf.FloorToInt((timePassed / combatTime) * resolution);

        //Don't run the same event index multiple times but run all events that still need to be run
        int catchUp = eventIndex - prevEventIndex;
        if (eventIndex != prevEventIndex)
        {
            foreach (TimelineEvent e in eventList)
            {
                if (!e.eventRun && (e.timelineIndex == eventIndex || (e.timelineIndex < eventIndex && e.timelineIndex > eventIndex-(catchUp+1)))) { RunEvent(e); }
            }
        }

        foreach (Enemy enemy in activeEnemies) { enemy.TimelineUpdate(); }

        UpdateVisuals();
        prevTimePassed = timePassed;
        timePassed += Time.deltaTime * combatSpeed;
        prevEventIndex = eventIndex;

        if (eventIndex > resolution) { EndCombat(); }
    }
    public void AddMarker(int index, EventType eventType, bool deleteOnActivate)
    {
        TimelineEvent newEvent = new TimelineEvent();
        newEvent.eventType = eventType;
        newEvent.timelineIndex = index;
        newEvent.deleteOnActivate = deleteOnActivate;

        RectTransform newMarker = Instantiate(markerPrefab, transform.GetChild(0)).GetComponent<RectTransform>();
        MarkerObject markObj = newMarker.GetComponent<MarkerObject>();
        markObj.SetType(eventType);
        newMarker.transform.localPosition = new Vector3(Mathf.Lerp(minMaxPos.x, minMaxPos.y, index / resolution), 0, 0);
        newMarker.gameObject.SetActive(true);

        newEvent.markerTrans = newMarker;
        
        eventList.Add(newEvent);
    }
    void RunEvent(TimelineEvent tEvent)
    {
        tEvent.eventRun = true;
        switch (tEvent.eventType)
        {
            case EventType.EnemyAtk: break;
            case EventType.PlayerAtk: break;
            case EventType.SpeedUp: break;
            case EventType.SpeedDown: break;
            case EventType.Delay: break;
            case EventType.ReverseStart: break;
            case EventType.ReverseEnd: break;
            case EventType.PortalStart: break;
            case EventType.PortalEnd: break;
        }
    }
}
