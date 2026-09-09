using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public Timeline timeline;
    public PlayerManager player;
    public Image attackCircle;
    public float dmg;
    public float atkSpd;
    float atkCooldown = 0;

    private void Start()
    {
        timeline.AddMarkers(atkSpd);
    }
    void Update()
    {
        if (atkCooldown >= 1)
        {
            Attack();
        }
        
        attackCircle.fillAmount = atkCooldown;

        atkCooldown += Time.deltaTime * atkSpd * timeline.combatSpeed;
    }
    void Attack()
    {
        player.MonsterAttacked(dmg);
        atkCooldown = 0f;
    }
}
