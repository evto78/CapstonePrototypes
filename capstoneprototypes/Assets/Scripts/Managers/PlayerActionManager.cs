using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionManager : MonoBehaviour
{
    public class StatusEffectInstance
    {
        public StatusEffect data;
        public int stacks;
    }

    [Header("Functional")]
    public int hp;
    public int mhp;

    public int maxEnergy;
    public int energy;

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

    public List<StatusEffectInstance> statEffects;
    public List<StatusEffect> statData;
    public List<PlayerSkill> equippedSkills;
    public List<PlayerSkill> skillData;

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

        statData.AddRange(Resources.LoadAll<StatusEffect>("StatusEffects"));
        SortStatusEffectData();

        skillData.AddRange(Resources.LoadAll<PlayerSkill>("Skills"));
        SortSkillData();
    }

    void Start()
    {
        hp = mhp;
        energy = maxEnergy;

        curParryCooldown = 0f;
        parryActive = false;
        parryLanded = false;

        curPerfectCooldown = 0f;
        perfectActive = false;
        perfectLanded = false;

        statEffects = new List<StatusEffectInstance>();

        if (equippedSkills == null || equippedSkills.Count < 4)
        {
            equippedSkills = new List<PlayerSkill>();
            equippedSkills.Add(skillData[0]);
            equippedSkills.Add(skillData[1]);
            equippedSkills.Add(skillData[2]);
            equippedSkills.Add(skillData[3]);
        }
    }

    public void SkillSelectStart()
    {
        energy = maxEnergy;
    }

    void SortStatusEffectData()
    {
        List<int> comparisonList = new List<int>();
        List<StatusEffect> sortedItemData = new List<StatusEffect>();
        for (int i = 0; i < statData.Count; i++) { comparisonList.Add(i); sortedItemData.Add(null); }
        for (int i = 0; i < statData.Count; i++)
        {
            sortedItemData[comparisonList.IndexOf(statData[i].id)] = statData[i];
        }
        statData = sortedItemData;
    }

    void SortSkillData()
    {
        List<int> comparisonList = new List<int>();
        List<PlayerSkill> sortedItemData = new List<PlayerSkill>();
        for (int i = 0; i < skillData.Count; i++) { comparisonList.Add(i); sortedItemData.Add(null); }
        for (int i = 0; i < skillData.Count; i++)
        {
            sortedItemData[comparisonList.IndexOf(skillData[i].id)] = skillData[i];
        }
        skillData = sortedItemData;
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
        fillBar.fillAmount = (float)hp / (float)mhp;
        if (parryLanded) { activeSprite = 3; }
        else if (perfectLanded) { activeSprite = 4; }
        else if (parryActive) { activeSprite = 2; }
        else if (curParryCooldown > 0) { activeSprite = 1; }
        else { activeSprite = 0; }
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
        PlayerSkill skill = attackEvent.playerSkill;
        float perfectMult = 1;

        if (skill.perfectable && perfectActive)
        {
            PerfectLanded();
            perfectMult = skill.perfectMultiplier;
        }

        switch (skill.id)
        {
            case 0: attackEvent.enemyTargeted.TakeDamage(Mathf.CeilToInt(skill.intensity * perfectMult)); break; //Bash
            case 1: attackEvent.enemyTargeted.TakeDamage(Mathf.CeilToInt(skill.intensity * perfectMult)); break; //Heavy Bash
            case 3: AddStatusEffect(0, Mathf.CeilToInt(skill.intensity * perfectMult)); break; //Focus
        }
    }

    void AddStatusEffect(int id, int stacks)
    {
        bool statusApplied = false;
        foreach(StatusEffectInstance effectInstance in statEffects)
        {
            if (effectInstance.data.id == id) { effectInstance.stacks += stacks; statusApplied = true; }
        }
        if (!statusApplied) 
        {
            StatusEffectInstance newEffectInstance = new StatusEffectInstance();
            newEffectInstance.stacks = stacks;
            newEffectInstance.data = statData[id];
            statEffects.Add(newEffectInstance); 
        }
    }

    void TakeDamage(int dmg)
    {
        hp -= dmg;
        if (hp <= 0) { Die(); }
    }

    void Die()
    {
        Debug.Log("DEAD!");
        gameObject.SetActive(false);
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
