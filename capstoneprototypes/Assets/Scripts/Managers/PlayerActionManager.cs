using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionManager : MonoBehaviour
{
    [Header("Functional")]
    public float hp;
    public float mhp;

    public float parryWindow;
    public float parryCooldown;
    float curParryCooldown = 0;
    bool parryActive;
    bool parryLanded;

    public float perfectWindow;
    public float perfectCooldown;
    float curPerfectCooldown = 0;
    bool perfectActive;
    bool perfectLanded;

    [Header("Visuals")]
    public List<ParticleSystem> parryParticles;
    public List<ParticleSystem> perfectParticles;
    public List<Sprite> sprites;
    int activeSprite = 0;

    [Header("References")]
    TimelineManager timeline;
    GeneralAnimator gAnim;
    public SpriteRenderer sr;
    public Image fillBar;

    private void Awake()
    {
        timeline = GameObject.Find("Timeline").GetComponent<TimelineManager>();
        gAnim = GetComponent<GeneralAnimator>();
    }

    void Start()
    {
        hp = mhp;

        curParryCooldown = 0f;
        parryActive = false;
        parryLanded = false;

        curPerfectCooldown = 0f;
        perfectActive = false;
        perfectLanded = false;
    }

    void Update()
    {
        //Manage Timers
        curParryCooldown -= Time.deltaTime; if (curParryCooldown < 0) { curParryCooldown = 0; }
        curPerfectCooldown -= Time.deltaTime; if (curPerfectCooldown < 0) { curPerfectCooldown = 0; }

        //Check if parry / perfect window is over
        parryActive = parryCooldown - curParryCooldown < parryWindow; if (!parryActive) { parryLanded = false; }
        perfectActive = perfectCooldown - curPerfectCooldown < perfectWindow; if (!perfectActive) { perfectLanded = false; }

        GetInputs();
        UpdateVisuals();
    }

    void GetInputs()
    {
        if (!timeline.combatPause && timeline.combatActive)
        {
            if (Input.GetMouseButtonDown(1)) { AttemptParry(); }
            if (Input.GetMouseButtonDown(0)) { AttemptPerfect(); }
        }
    }

    void AttemptParry()
    {
        if (curParryCooldown > 0 && !parryLanded) { return; }

        curParryCooldown = parryCooldown;
        parryActive = true;
    }

    void AttemptPerfect()
    {
        if (curPerfectCooldown > 0 && !perfectLanded) { return; }

        curPerfectCooldown = perfectCooldown;
        perfectActive = true;
    }

    void UpdateVisuals()
    {
        sr.sprite = sprites[activeSprite];
    }

    public IEnumerator ReactableEvent(TimelineManager.TimelineEvent onComingEvent)
    {
        //If player is already parrying, then resolve this now.
        if (onComingEvent.eventType == TimelineManager.EventType.EnemyAtk)
        {
            if (parryActive) { MonsterAttacked(onComingEvent, 0f); }
            else
            {
                //Otherwise, Wait for up to half of the parry window, and check again.
                bool attackProcessed = false;
                float tempTimer = parryWindow / 2f;
                while (tempTimer > 0 && attackProcessed == false)
                {
                    if (parryActive) { attackProcessed = true; MonsterAttacked(onComingEvent, (parryWindow / 2f) - tempTimer); }
                    tempTimer -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                //If the attack still wasn't parried, send it now.
                if (!attackProcessed) { MonsterAttacked(onComingEvent, (parryWindow / 2f) - tempTimer); }
            }
        }

        //If player is already perfecting, then resolve this now.
        else if (onComingEvent.eventType == TimelineManager.EventType.PlayerAtk)
        {
            if (perfectActive) { PlayerActivateSkill(onComingEvent, 0f); }
            else
            {
                //Otherwise, Wait for up to half of the perfect window, and check again.
                bool attackProcessed = false;
                float tempTimer = perfectWindow / 2f;
                while (tempTimer > 0 && attackProcessed == false)
                {
                    if (perfectActive) { attackProcessed = true; PlayerActivateSkill(onComingEvent, (perfectWindow / 2f) - tempTimer); }
                    tempTimer -= Time.deltaTime;
                    yield return new WaitForEndOfFrame();
                }
                //If the attack still wasn't parried, send it now.
                if (!attackProcessed) { PlayerActivateSkill(onComingEvent, (perfectWindow / 2f) - tempTimer); }
            }
        }

        yield return null;
    }

    //An enemy is about to hit the player
    public void MonsterAttacked(TimelineManager.TimelineEvent attackEvent, float delay)
    {
        if (parryActive)
        {
            ParryLanded();
        }
        else
        {
            TakeDamage(attackEvent.enemyAction.damage);
        }
    }

    //One of the players skills are about to activate
    public void PlayerActivateSkill(TimelineManager.TimelineEvent attackEvent, float delay)
    {
        if (attackEvent.playerSkill.perfectable && perfectActive)
        {
            PerfectLanded();
        }

        PlayerSkill skill = attackEvent.playerSkill;
        switch (skill.id)
        {
            case 0: break;
        }
    }

    void TakeDamage(float dmg)
    {
        hp -= dmg;
        if (hp <= 0) { Die(); }
    }

    void Die()
    {
        Debug.Log("DEAD!");
    }

    void ParryLanded()
    {
        parryLanded = true;
        foreach (ParticleSystem ps in parryParticles) { ps.Stop(); ps.Play(); }
        UpdateVisuals();
    }

    void PerfectLanded()
    {
        perfectLanded = true;
        foreach (ParticleSystem ps in perfectParticles) { ps.Stop(); ps.Play(); }
        UpdateVisuals();
    }
}
