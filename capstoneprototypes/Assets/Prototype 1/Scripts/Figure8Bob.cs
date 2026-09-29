using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Figure8Bob : MonoBehaviour
{
    public float offset;
    public float speed;
    public float intensity;
    Vector3 startPos;
    float timer;
    private void Start()
    {
        startPos = transform.localPosition;
        timer = offset;
    }
    void Update()
    {
        float xBob = Mathf.Sin(timer * 360f);
        float yBob = Mathf.Sin(timer * 720f);
        Vector3 tarPos = new Vector3(startPos.x + xBob * intensity, startPos.y + yBob * intensity, startPos.z);

        transform.localPosition = tarPos;

        timer += Time.deltaTime * speed;
        if (timer > 1) { timer -= 1; }
    }
}
