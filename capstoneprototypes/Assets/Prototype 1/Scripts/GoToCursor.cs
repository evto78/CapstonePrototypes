using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCursor : MonoBehaviour
{
    public float speed;
    void Update()
    {
        Vector3 flatPos = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        transform.position = Vector3.Lerp(transform.position, flatPos, Time.deltaTime * speed);
    }
}
