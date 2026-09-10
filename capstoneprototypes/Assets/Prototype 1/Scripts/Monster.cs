using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    public SpriteRenderer mySprite;
    public List<Sprite> sprites;
    public Timeline timeline;
    public PlayerManager player;
    public Image attackCircle;
    public float dmg;
    public float atkSpd;
    float atkCooldown = 0;

    private void Start()
    {
        timeline.AddMarkers(atkSpd);
        mySprite.sprite = sprites[0];
    }
    void Update()
    {
        if (atkCooldown >= 1)
        {
            mySprite.sprite = sprites[1];
            Attack();
        }
        else if (atkCooldown < 0.1f)
        {
            //mySprite.sprite = sprites[1];
        }
        else
        {
            mySprite.sprite = sprites[0];
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
