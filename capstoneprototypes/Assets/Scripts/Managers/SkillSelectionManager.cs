using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelectionManager : MonoBehaviour
{
    public List<GameObject> skillButtons;
    public TextMeshProUGUI readout;
    PlayerActionManager player;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerActionManager>();
    }

    void Update()
    {
        bool hovering = false;
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (IsTouchingMouse(skillButtons[i])) 
            {
                hovering = true;
                readout.text = player.equippedSkills[i].skillDescription;
            }
        }
        if (!hovering) { readout.text = "..."; }

        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        TextMeshProUGUI skillTXT;

        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillTXT = skillButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            skillTXT.text = player.equippedSkills[i].skillName;
        }
    }

    public bool IsTouchingMouse(GameObject g)
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return g.GetComponent<Collider2D>().OverlapPoint(point);
    }
}
