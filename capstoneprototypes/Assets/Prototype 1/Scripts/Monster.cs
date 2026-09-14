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
    public float atkInterval;
    float intervalCounter;
    float atkCooldown = 0;

    private void Start()
    {
        timeline.AddMarkers(atkInterval);
        mySprite.sprite = sprites[0];
        intervalCounter = 0;
    }
    void Update()
    {
        intervalCounter += Time.deltaTime * timeline.combatSpeed;

        if (intervalCounter >= atkInterval)
        {
            mySprite.sprite = sprites[1];
            Attack();
        }
        else if (intervalCounter < atkInterval / 10f)
        {
            //mySprite.sprite = sprites[1];
        }
        else
        {
            mySprite.sprite = sprites[0];
        }
        
        attackCircle.fillAmount = intervalCounter/atkInterval;
    }
    void Attack()
    {
        player.MonsterAttacked(dmg);
        intervalCounter = 0f;
    }
}
