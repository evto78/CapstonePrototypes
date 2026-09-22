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
    }
    [System.Serializable]
    public enum EventType { EnemyAtk, PlayerAtk, SpeedUp, SpeedDown, Delay, ReverseStart, ReverseEnd, PortalStart, PortalEnd}
    public List<TimelineEvent> eventList;
    public float combatTime;
    public float timePassed;
    public int eventIndex;
    public int roundNumber;
    public float combatSpeed;
    public bool combatActive;
    [Header("Visuals")]
    public Image fillBar;
    public GameObject markerPrefab;
    public Vector2 minMaxPos;
    public Transform thresholdBar;
    public bool fillPixelByPixel;

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
            fillBar.fillAmount = eventIndex / 160f;
        }
        else
        {
            fillBar.fillAmount = timePassed / combatTime;
        }
    }
    public void StartCombat()
    {
        if (combatActive) { roundNumber++; } else { roundNumber = 0; }
        eventList = new List<TimelineEvent>();
        combatActive = true;
        timePassed = 0;
    }
    public void EndCombat()
    {
        combatActive = false;
        timePassed = 0;
        roundNumber = 0;
    }
    void UpdateCombat()
    {
        eventIndex = Mathf.FloorToInt((timePassed / combatTime) * 160);
        Debug.Log(eventIndex);

        foreach(TimelineEvent e in eventList)
        {
            if (!e.eventRun && e.timelineIndex <= eventIndex) { RunEvent(e); }
        }

        UpdateVisuals();
        timePassed += Time.deltaTime * combatSpeed;
    }
    public void AddMarker(int index, EventType eventType, bool deleteOnActivate)
    {
        TimelineEvent newEvent = new TimelineEvent();
        newEvent.eventType = eventType;
        newEvent.timelineIndex = index;
        newEvent.deleteOnActivate = deleteOnActivate;
        eventList.Add(newEvent);
    }
    void RunEvent(TimelineEvent tEvent)
    {
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
