using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

//Base class for all enemies
public class Enemy : MonoBehaviour
{
    [System.Serializable]
    public class Attack
    {
        public float damage;
        public int animWindupStart; //The timeline index position where this enemy changes their sprite to prepare for the attack.
        public int animWinddownEnd; //The timeline index position where this enemy changes their sprite back to idle after the attack.
    }

    [System.Serializable]
    public class AttackPattern
    {
        public List<Attack> attackSequence = new List<Attack>();
    }

    public List<Sprite> spriteList; //0: idle, 1: windup, 2: attack, 3: winddown, 4: hurt, 5+ is for extra sprites / attacks.
    public List<AttackPattern> attackPatterns;

    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
