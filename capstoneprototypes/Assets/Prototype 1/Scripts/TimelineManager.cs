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
    }
    [System.Serializable]
    public enum EventType { EnemyAtk, PlayerAtk, SpeedUp, SpeedDown, Delay, ReverseStart, ReverseEnd, PortalStart, PortalEnd}
    public List<TimelineEvent> eventList;
    public float combatTime;
    float timePassed;
    int eventIndex;
    public int roundNumber;
    public float combatSpeed;
    public bool combatActive;

    private void Start()
    {
        combatActive = false;
    }
    private void Update()
    {
        if (combatActive) { UpdateCombat(); }
        UpdateVisuals();
    }
    void UpdateVisuals()
    {

    }
    public void StartCombat()
    {
        if (combatActive) { roundNumber++; } else { roundNumber = 0; }
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
        //eventIndex = 

        //timePassed += Time.deltaTime * combatSpeed;
    }
}
