using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class RoomSelection : MonoBehaviour
{
    public List<Sprite> roomIcons;
    public List<Sprite> selectedRoomIcons;
    public Sprite thoughtBubbleSprite;
    public Sprite selectedThoughtBubbleSprite;
    public List<SpriteRenderer> optionSprites;
    public List<Vector3> paths = new List<Vector3>();
    public int distance;
    void Start()
    {
        distance = 0;
        if (paths.Count < 1) { GeneratePaths(); }

        DisplayPaths();
    }
    void GeneratePaths()
    {
        for (int i = 0; i < 10; i++)
        {
            paths.Add(new Vector3(Random.Range(0, roomIcons.Count), Random.Range(0, roomIcons.Count), Random.Range(0, roomIcons.Count)));
        }
    }
    void DisplayPaths()
    {
        if (paths[distance].x < 0 || paths[distance].y < 0 || paths[distance].z < 0) { return; }

        if (IsTouchingMouse(optionSprites[0].gameObject))
        { optionSprites[0].sprite = selectedThoughtBubbleSprite; optionSprites[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = selectedRoomIcons[Mathf.RoundToInt(paths[distance].x)]; }
        else { optionSprites[0].sprite = thoughtBubbleSprite; optionSprites[0].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = roomIcons[Mathf.RoundToInt(paths[distance].x)]; }

        if (IsTouchingMouse(optionSprites[1].gameObject))
        { optionSprites[1].sprite = selectedThoughtBubbleSprite; optionSprites[1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = selectedRoomIcons[Mathf.RoundToInt(paths[distance].y)]; }
        else { optionSprites[1].sprite = thoughtBubbleSprite; optionSprites[1].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = roomIcons[Mathf.RoundToInt(paths[distance].y)]; }

        if (IsTouchingMouse(optionSprites[2].gameObject))
        { optionSprites[2].sprite = selectedThoughtBubbleSprite; optionSprites[2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = selectedRoomIcons[Mathf.RoundToInt(paths[distance].z)]; }
        else { optionSprites[2].sprite = thoughtBubbleSprite; optionSprites[2].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = roomIcons[Mathf.RoundToInt(paths[distance].z)]; }
    }
    public bool IsTouchingMouse(GameObject g)
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return g.GetComponent<Collider2D>().OverlapPoint(point);
    }
    void Update()
    {
        DisplayPaths();

        if (Input.GetMouseButtonDown(0))
        {
            if (IsTouchingMouse(optionSprites[0].gameObject)) { SelectRoom(0); }
            else if (IsTouchingMouse(optionSprites[1].gameObject)) { SelectRoom(1); }
            else if (IsTouchingMouse(optionSprites[2].gameObject)) { SelectRoom(2); }
        }
    }
    void SelectRoom(int option)
    {
        //Go to the selected room!
        //Mark the path as selected by making the number negative. Add 0.1 before doing so, so that 0 can be turned negative.
        switch (option)
        {
            case 0: paths[distance] = new Vector3(-(paths[distance].x + 0.1f), paths[distance].y, paths[distance].z); break;
            case 1: paths[distance] = new Vector3(paths[distance].x, -(paths[distance].y + 0.1f), paths[distance].z); break;
            case 2: paths[distance] = new Vector3(paths[distance].x, paths[distance].y, -(paths[distance].z + 0.1f)); break;
        }

        distance++;

        if (distance >= paths.Count)
        {
            GeneratePaths();
        }

        DisplayPaths();
    }
}
