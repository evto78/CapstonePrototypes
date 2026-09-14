using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timeline : MonoBehaviour
{
    public GameObject marker;
    List<Transform> activeMarkers;
    public Vector2 minMax;
    public float combatTime;
    public float combatSpeed;
    float combatTimeLeft;
    public Image fillBar;
    public Transform thresholdBar;

    public bool speedUp;
    public float speedUpAmt;
    public float speedUpTime;
    private void Start()
    {
        marker.SetActive(false);
        StartCombat();
    }

    public void StartCombat() 
    {
        combatTimeLeft = combatTime;
        fillBar.fillAmount = 0;
    }

    private void Update()
    {
        combatTimeLeft -= Time.deltaTime * combatSpeed;

        UpdateVisual();
    }
    void UpdateVisual()
    {
        float trueFillAmt = 1 - (combatTimeLeft / combatTime);
        float timelineRes = 160;
        fillBar.fillAmount = Mathf.Round(trueFillAmt * timelineRes) / timelineRes;
        thresholdBar.transform.localPosition = new Vector3(Mathf.Lerp(minMax.x, minMax.y, fillBar.fillAmount), 0, 0);
        thresholdBar.gameObject.SetActive(thresholdBar.transform.localPosition.x > minMax.x + 3);
    }
    public void AddMarkers(float atkInterval)
    {
        float tempCounter = 0f;

        if (atkInterval <= 0) {Debug.Log("Interval is less than 0!!"); return; }

        while(tempCounter <= combatTime)
        {
            tempCounter += atkInterval;
            if (tempCounter <= combatTime)
            {
                Transform newMarker = Instantiate(marker, transform.GetChild(0).transform).transform;
                newMarker.transform.localPosition = new Vector3(Mathf.Lerp(minMax.x, minMax.y, tempCounter/combatTime), 0, 0);
                newMarker.gameObject.SetActive(true);
            }
        }
    }
}
