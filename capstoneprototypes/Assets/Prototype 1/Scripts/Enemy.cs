using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Base class for all enemies
public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class Action
    {
        public enum ActionType { Attack, Delay, SpeedUp, SlowDown}
        public ActionType actionType;
        public float damage; //If the action is an attack, how much damage should it do? If the action is speed up / down, how much should the combat time change by?

        //Try to make the delay in 160ths. As a tip, 0.05 is 8/160, and so 0.025 is 4/160. Not strictly needed, but helps make timing make more sense!
        public float delayBeforeAction; //How long should this enemy wait before doing this action?
        public float delayAfterAction; //How long should this enemy wait after doing this action?

        public bool playAnim; //Should this action change the enemies sprite at all.
        //Make sure that the wind up and wind down fits within the delay before after the action. 
        //Set both to 0 if you don't want an animaiton change \/
        public float animWindupStart; //The amount of time before the end of the "delayBeforeAction" where the enemy changes their sprite to prepare for an attack.
        public float animWinddownEnd; //The amount of time after the end of the "delayBeforeAction" where the enemy changes their sprite to attack.
    }

    [System.Serializable]
    public class AttackPattern
    {
        public List<Action> actionSequence = new List<Action>();
    }

    [Header("Visuals")]
    public List<Sprite> spriteList; //0: idle, 1: windup, 2: attack, 3: hurt, 4+ is for extra sprites / attacks.
    public SpriteRenderer sr;
    public Image attackCircle;
    int activeSprite;

    [Header("Functional")]
    TimelineManager timeline;
    float curTimePassed; //The amount of time passed according to the timeline. Accurate to all timeline distortions.
    float curTimelineIndex; //The amount of time passed according to the current index of the timeline relative to its resolution. Accurate to all timeline distortions.
    PlayerManager player;
    public List<AttackPattern> attackPatterns;
    int curRound;
    public List<int> patternByRound;

    void Start()
    {
        timeline = GameObject.Find("Timeline").GetComponent<TimelineManager>();
        player = GameObject.Find("Player").GetComponent<PlayerManager>();

        timeline.activeEnemies.Add(this);
        curTimePassed = 0;

        activeSprite = 0;

        SetupMarkers();
    }

    private void OnDisable()
    {
        if (timeline != null && timeline.activeEnemies.Contains(this))
        {
            timeline.activeEnemies.Remove(this);
        }
    }

    private void OnDestroy()
    {
        if (timeline != null && timeline.activeEnemies.Contains(this))
        {
            timeline.activeEnemies.Remove(this);
        }
    }

    void SetupMarkers()
    {
        foreach (AttackPattern pattern in attackPatterns)
        {
            float accumulatedDelay = 0;
            foreach (Action action in pattern.actionSequence)
            {
                accumulatedDelay += action.delayBeforeAction;
                int indexOfDelay = Mathf.FloorToInt((accumulatedDelay / timeline.combatTime) * timeline.resolution);
                switch (action.actionType)
                {
                    case Action.ActionType.Attack: timeline.AddMarker(indexOfDelay, TimelineManager.EventType.EnemyAtk, false); break;
                    case Action.ActionType.SpeedUp: timeline.AddMarker(indexOfDelay, TimelineManager.EventType.SpeedUp, true); break;
                    case Action.ActionType.SlowDown: timeline.AddMarker(indexOfDelay, TimelineManager.EventType.SpeedDown, true); break;
                }
                accumulatedDelay += action.delayAfterAction;
            }
        }
    }

    //Update as normal, but sent from the timeline. This is to make sure that timing is accurate.
    public void TimelineUpdate()
    {
        curTimePassed = timeline.timePassed;
        curTimelineIndex = timeline.eventIndex;
        curRound = timeline.roundNumber;

        FindCurState();

        UpdateVisuals();
    }

    void FindCurState()
    {
        int relitiveRound = curRound % patternByRound.Count;

        AttackPattern curPattern = attackPatterns[patternByRound[relitiveRound]];

        float accumulatedDelay = 0;
        foreach (Action action in curPattern.actionSequence)
        {
            if (curTimePassed > accumulatedDelay && curTimePassed < accumulatedDelay + action.delayBeforeAction + action.delayAfterAction)
            {
                RunCurAction(action, curTimePassed - accumulatedDelay, timeline.prevTimePassed - accumulatedDelay); break;
            }
            else
            {
                accumulatedDelay += action.delayBeforeAction + action.delayAfterAction;
            }
        }
    }

    void RunCurAction(Action action, float relitiveTime, float prevRelitiveTime)
    {
        if (relitiveTime < action.delayBeforeAction)
        {
            //before delay
            if ((relitiveTime > action.delayBeforeAction - action.animWindupStart) && action.playAnim)
            {
                activeSprite = 1; //prepare for attack
            }
        }
        else if (prevRelitiveTime < action.delayBeforeAction)
        {
            //action!
            if (action.playAnim) { activeSprite = 2; }
            Debug.Log("ACTION!");
            Debug.Log("Enemy: " + gameObject.name);
            Debug.Log("CurTime: " + curTimePassed);
            Debug.Log("CurIndex: " + curTimelineIndex);
            Debug.Log("CurRound: " + curRound);
            Debug.Log("ActionType: " + action.actionType);
        }
        else
        {
            //after delay
            if (relitiveTime < action.delayBeforeAction + action.animWinddownEnd && action.playAnim)
            {
                activeSprite = 2; //attack anim
            }
        }
    }

    void UpdateVisuals()
    {
        sr.sprite = spriteList[activeSprite];
    }
}
