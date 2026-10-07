using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChangeOnMouse : MonoBehaviour
{
    Image sr;
    public List<Sprite> sprites;

    float timer = 0f;

    private void Start()
    {
        sr = GetComponent<Image>();
        sr.sprite = sprites[0];
    }
    void Update()
    {
        if (timer > 0) { timer -= Time.deltaTime; sr.sprite = sprites[2]; }
        else
        {
            if (IsTouchingMouse(gameObject) && Input.GetMouseButton(0)) { sr.sprite = sprites[2]; timer = 2f; }
            else if (IsTouchingMouse(gameObject)) { sr.sprite = sprites[1]; }
            else { sr.sprite = sprites[0]; }
        }
    }
    public bool IsTouchingMouse(GameObject g)
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return g.GetComponent<Collider2D>().OverlapPoint(point);
    }
}
