using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelectionManager : MonoBehaviour
{
    public List<GameObject> skillButtons;
    public TextMeshProUGUI readout;
    public TextMeshProUGUI energyText;
    PlayerActionManager player;

    public List<ReadoutOnHover> readoutObjects;

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
        if (!hovering)
        {
            foreach (ReadoutOnHover readoutObj in readoutObjects)
            {
                if (IsTouchingMouse(readoutObj.gameObject))
                {
                    readout.text = readoutObj.readoutText;
                    hovering = true;
                }
            }
        }
        if (!hovering) { readout.text = "..."; }

        UpdateVisuals();
    }

    void UpdateVisuals()
    {
        TextMeshProUGUI skillText;

        for (int i = 0; i < skillButtons.Count; i++)
        {
            skillText = skillButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            skillText.text = player.equippedSkills[i].skillName;

            if (player.equippedSkills[i].cost > player.energy) { skillButtons[i].GetComponentInChildren<Button>().interactable = false; } else { skillButtons[i].GetComponentInChildren<Button>().interactable = true; }
        }

        energyText.text = player.energy.ToString();
    }

    public bool IsTouchingMouse(GameObject g)
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return g.GetComponent<Collider2D>().OverlapPoint(point);
    }
}
