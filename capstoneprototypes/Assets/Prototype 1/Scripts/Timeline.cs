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
        fillBar.fillAmount = 1 - (combatTimeLeft / combatTime);
        if (fillBar.fillAmount > speedUpTime && !speedUp)
        {
            speedUp = true;
            combatSpeed *= speedUpAmt;
        }
    }
    public void AddMarkers(float atkSpd)
    {
        float tempCounter = 0f;
        float interval = 1f / atkSpd;

        while(tempCounter <= combatTime)
        {
            tempCounter += interval;
            if (tempCounter <= combatTime)
            {
                Transform newMarker = Instantiate(marker, transform.GetChild(0).transform).transform;
                newMarker.transform.localPosition = new Vector3(Mathf.Lerp(minMax.x, minMax.y, tempCounter/combatTime), 0, 0);
                newMarker.gameObject.SetActive(true);
            }
        }
    }
}
