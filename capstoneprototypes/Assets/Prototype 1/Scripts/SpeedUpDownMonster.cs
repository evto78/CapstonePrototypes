using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedUpDownMonster : MonoBehaviour
{
    public SpriteRenderer mySprite;
    public List<Sprite> sprites;
    public TimelineManager timeline;
    public PlayerManager player;
    public Image attackCircle;
    public float dmg;
    public float atkInterval;
    float intervalCounter;

    void Start()
    {
        timeline = GameObject.Find("Timeline").GetComponent<TimelineManager>();
        timeline.AddEnemyAtkMarkers(atkInterval, TimelineManager.EventType.SpeedUp);
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

        attackCircle.fillAmount = intervalCounter / atkInterval;
    }
    void Attack()
    {
        player.MonsterAttacked(dmg);
        intervalCounter = 0f;
        timeline.combatSpeed += 0.5f;
    }
}
