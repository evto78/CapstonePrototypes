using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillSelectionManager : MonoBehaviour
{
    public List<GameObject> skillButtons;
    public TextMeshProUGUI readout;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool hovering = false;
        for (int i = 0; i < skillButtons.Count; i++)
        {
            if (IsTouchingMouse(skillButtons[i])) 
            {
                hovering = true;
                readout.text = "You are looking at skill " + (i+1);
            }
        }
        if (!hovering) { readout.text = "..."; }
    }

    public bool IsTouchingMouse(GameObject g)
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return g.GetComponent<Collider2D>().OverlapPoint(point);
    }
}
