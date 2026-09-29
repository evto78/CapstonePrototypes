using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralAnimator : MonoBehaviour
{
    public bool isLocal;

    Vector3 startPos;
    Quaternion startRot;
    Vector3 startScale;

    private void Awake()
    {
        startPos = GetPos();
        startRot = GetRot();
        startScale = GetScale();
    }

    Vector3 GetPos()
    { if (isLocal) { return transform.localPosition; } else { return transform.position; } }
    Quaternion GetRot()
    { if (isLocal) { return transform.localRotation; } else { return transform.rotation; } }
    Vector3 GetScale()
    { return transform.localScale; }

    public void ResetAll()
    {
        StopAllCoroutines();
         
        if (isLocal)
        {
            transform.localPosition = startPos;
            transform.localRotation = startRot;
            transform.localScale = startScale;
        }
        else
        {
            transform.position = startPos;
            transform.rotation = startRot;
            transform.localScale = startScale;
        }
    }

    public IEnumerator GlideToPos(Vector3 newPos, float seconds, bool relativePos)
    {
        Vector3 curPos = GetPos();
        Vector3 tarPos = newPos;
        if (relativePos) { tarPos += curPos; }
        float timer = seconds;
        Vector3 lerpPos;

        while (timer > 0)
        {
            lerpPos = Vector3.Lerp(tarPos, curPos, timer/seconds);
            if (isLocal) { transform.localPosition = lerpPos; } else { transform.position = lerpPos; }
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
    public IEnumerator TurnToRot(Quaternion newRot, float seconds, bool relativeRot)
    {
        Quaternion curRot = GetRot();
        Quaternion tarRot = newRot;
        if (relativeRot) { tarRot *= curRot; }
        float timer = seconds;
        Quaternion lerpRot;

        while (timer > 0)
        {
            lerpRot = Quaternion.Lerp(tarRot, curRot, timer / seconds);
            if (isLocal) { transform.localRotation = lerpRot; } else { transform.rotation = lerpRot; }
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
    public IEnumerator LerpScale(Vector3 newScale, float seconds, bool relativeScale)
    {
        Vector3 curScale = GetScale();
        Vector3 tarScale = newScale;
        if (relativeScale) { tarScale += curScale; }
        float timer = seconds;
        Vector3 lerpScale;

        while (timer > 0)
        {
            lerpScale = Vector3.Lerp(tarScale, curScale, timer / seconds);
            transform.localScale = lerpScale;
            timer -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        yield return null;
    }
    //Recursively move in this direction every frame.
    public IEnumerator ContinuousGlide(Vector3 dir)
    {
        if (isLocal) { transform.localPosition += dir * Time.deltaTime; } 
        else { transform.position += dir * Time.deltaTime; }
        yield return ContinuousGlide(dir);
    }
    //Recursively turn in this direction every frame.
    public IEnumerator ContinuousTurn(Vector3 dir)
    {
        if (isLocal) { transform.localEulerAngles += dir * Time.deltaTime; }
        else { transform.eulerAngles += dir * Time.deltaTime; }
        yield return ContinuousTurn(dir);
    }
    //Recursively move in this direction every frame.
    public IEnumerator ContinuousScale(Vector3 scale)
    {
        transform.localScale += scale * Time.deltaTime;
        yield return ContinuousScale(scale);
    }
}
