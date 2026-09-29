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
    bool blockActive;

    public float perfectWindow;
    public float perfectCooldown;
    float curPerfectCooldown = 0;
    bool perfectActive;
    bool perfectLanded;
    bool attackActive;

    [Header("Visuals")]
    public List<ParticleSystem> parryParticles;
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
        blockActive = false;

        curPerfectCooldown = 0f;
        perfectActive = false;
        perfectLanded = false;
        attackActive = false;
    }

    void Update()
    {
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


    }

    void AttemptPerfect()
    {
        if (curPerfectCooldown > 0 && !perfectLanded) { return; }
    }

    void UpdateVisuals()
    {
        sr.sprite = sprites[activeSprite];
    }
}
