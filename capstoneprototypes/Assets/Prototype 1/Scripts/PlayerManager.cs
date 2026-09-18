using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public List<ParticleSystem> parryParticles;
    public Image fillBar;
    public List<Sprite> sprites;
    public SpriteRenderer sr;
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

    public bool combatActive;

    private void Start()
    {
        hp = mhp;
    }

    void Update()
    {
        if (curParryCooldown > 0)
        {
            if (curParryCooldown < parryCooldown - parryWindow) { parryActive = false; parryLanded = false; }

            curParryCooldown -= Time.deltaTime;
        }

        if (curPerfectCooldown > 0)
        {
            if (curPerfectCooldown < perfectCooldown - perfectWindow) { perfectActive = false; perfectLanded = false; }

            curPerfectCooldown -= Time.deltaTime;
        }

        if (combatActive)
        {
            //Parry
            if (Input.GetMouseButtonDown(1))
            {
                if (curParryCooldown <= 0 || parryLanded)
                {
                    curParryCooldown = parryCooldown;
                    parryActive = true; blockActive = true;
                }
            }
            else if (Input.GetMouseButtonUp(1)) { blockActive = false; }

            //Attack
            if (Input.GetMouseButtonDown(0))
            {
                if (curPerfectCooldown <= 0 || perfectLanded)
                {
                    curPerfectCooldown = perfectCooldown;
                    perfectActive = true; attackActive = true;
                }
            }
            else if (Input.GetMouseButtonUp(0)) { attackActive = false; }
        }

        UpdateVisuals();
    }
    void UpdateVisuals()
    {
        fillBar.fillAmount = hp / mhp;
        if (parryLanded)
        {
            sr.sprite = sprites[3];
        }
        else if (parryActive)
        {
            sr.sprite = sprites[2];
        }
        else if (blockActive)
        {
            sr.sprite = sprites[1];
        }
        else
        {
            sr.sprite = sprites[0];
        }
    }
    public void MonsterAttacked(float dmg)
    {
        if (parryActive)
        {
            ParryLanded();
        }
        else if (blockActive)
        {
            hp -= dmg / 2f;
        }
        else
        {
            hp -= dmg;
        }

        if (hp <= 0) { gameObject.SetActive(false); }
    }
    void ParryLanded()
    {
        foreach(ParticleSystem ps in parryParticles) {ps.Stop(); ps.Play(); }
        parryLanded = true;
        UpdateVisuals();
    }

    //Needs to be called when the chosen attack is planned to land !!!NOT HOOKED UP TO ANYTHING YET!!!
    public void AboutToAttack()
    {
        if (perfectActive)
        {
            //made a perfect attack
        }
        else if (attackActive)
        {
            //made a ok attack
        }
        else
        {
            //missed the attack window, made a bad attack
        }
    }
}
