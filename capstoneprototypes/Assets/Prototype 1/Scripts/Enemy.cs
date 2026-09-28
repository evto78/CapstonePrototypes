using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

//Base class for all enemies
public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class Action
    {
        public enum ActionType { Attack, Delay, SpeedUp, SpeedDown}
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

    private void Awake()
    {
        timeline = GameObject.Find("Timeline").GetComponent<TimelineManager>();
        player = GameObject.Find("Player").GetComponent<PlayerManager>();

        timeline.activeEnemies.Add(this);
    }

    void Start()
    {
        curTimePassed = 0;
        activeSprite = 0;
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

    public void SetupMarkers()
    {
        int relitiveRound = curRound % patternByRound.Count;

        AttackPattern curPattern = attackPatterns[patternByRound[relitiveRound]];

        float accumulatedDelay = 0;
        while (accumulatedDelay <= timeline.combatTime)
        {
            foreach (Action action in curPattern.actionSequence)
            {
                accumulatedDelay += action.delayBeforeAction;
                if (accumulatedDelay > timeline.combatTime) { break; }
                int indexOfDelay = Mathf.FloorToInt((accumulatedDelay / timeline.combatTime) * timeline.resolution);
                switch (action.actionType)
                {
                    case Action.ActionType.Attack: timeline.AddMarker(indexOfDelay, TimelineManager.EventType.EnemyAtk, false, this, action); break;
                    case Action.ActionType.SpeedUp: timeline.AddMarker(indexOfDelay, TimelineManager.EventType.SpeedUp, true, this, action); break;
                    case Action.ActionType.SpeedDown: timeline.AddMarker(indexOfDelay, TimelineManager.EventType.SpeedDown, true, this, action); break;
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
        bool actionFound = false;
        int loopLimit = 1000;
        while (!actionFound)
        {
            foreach (Action action in curPattern.actionSequence)
            {
                if (curTimePassed >= accumulatedDelay && curTimePassed < accumulatedDelay + action.delayBeforeAction + action.delayAfterAction)
                {
                    actionFound = true;
                    RunCurAction(action, curTimePassed - accumulatedDelay, timeline.prevTimePassed - accumulatedDelay); break;
                }
                else
                {
                    accumulatedDelay += action.delayBeforeAction + action.delayAfterAction;
                }
            }
            loopLimit--;
            if (loopLimit < 1) { Debug.LogError("ENEMY: " + gameObject.name + ", NO ACTION COULD BE FOUND!"); break; }
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
            if (action.playAnim)
            {
                attackCircle.fillAmount = relitiveTime / action.delayBeforeAction;
            }
        }
        else if (prevRelitiveTime < action.delayBeforeAction)
        {
            //action!
            if (action.playAnim) 
            { 
                activeSprite = 2; 
                attackCircle.fillAmount = 1; 
            }

            //ActivateAction(action);
        }
        else
        {
            //after delay
            if (relitiveTime < action.delayBeforeAction + action.animWinddownEnd && action.playAnim)
            {
                activeSprite = 2; //attack anim
                attackCircle.fillAmount = 1;
            }
            else if (action.playAnim)
            {
                activeSprite = 0; //idle anim
                attackCircle.fillAmount = 0;
            }
        }
    }

    void UpdateVisuals()
    {
        sr.sprite = spriteList[activeSprite];
    }

    public void ActivateAction(Action action)
    {
        switch (action.actionType)
        {
            case Action.ActionType.Attack: player.MonsterAttacked(action.damage); break;
            case Action.ActionType.SpeedUp: timeline.combatSpeed += action.damage; break;
            case Action.ActionType.SpeedDown: timeline.combatSpeed -= action.damage; break;
        }
    }
}
