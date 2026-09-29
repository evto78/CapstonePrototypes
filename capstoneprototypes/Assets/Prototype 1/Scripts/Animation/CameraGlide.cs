using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGlide : MonoBehaviour
{
    Vector3 startPos;
    public Vector3 endPos;

    public AnimationCurve glideCurve;

    public Transform timelinePivot;
    public AnimationCurve timelineCurve;

    public bool isUp;

    public float glideSpeed;
    float timer;
    private void Start()
    {
        startPos = transform.position;
        timer = 0f;
    }
    private void Update()
    {
        if (isUp)
        {
            timer -= Time.deltaTime * glideSpeed;
        }
        else
        {
            timer += Time.deltaTime * glideSpeed;
        }

        timer = Mathf.Clamp(timer, 0f, 1f);

        transform.position = Vector3.Lerp(startPos, endPos, glideCurve.Evaluate(timer));
        //timelinePivot.localEulerAngles = Vector3.forward * 90f * timelineCurve.Evaluate(timer);
    }
}
