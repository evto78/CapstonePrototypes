using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BGParralax : MonoBehaviour
{
    List<Transform> layers; //BG is 0, FG is max.
    List<float> layerWeight;

    public bool pixelPerfect;

    public Vector2 minMaxXOffset; //The most that X can sway left, and right.
    public Vector2 minMaxYOffset; //The most that Y can sway up, and down.

    float xTimer = 0.5f;
    float yTimer = 0.5f;

    float xIn = 0f;
    float yIn = 0f;

    Vector2 delayInput;

    public Vector2 speed;
    public Vector2 inputIntensity;

    private void Start()
    {
        delayInput = Vector2.zero;

        layers = new List<Transform>();
        layerWeight = new List<float>();

        for(int i = 0; i < transform.childCount; i++)
        {
            layers.Add(transform.GetChild(i));
            layerWeight.Add(1f);
        }

        for(int i = 0; i < layerWeight.Count; i++)
        {
            layerWeight[i] = (float)((layerWeight.Count-1f) - i) / ((float)layerWeight.Count-1f);
            if (i == layerWeight.Count - 1) { layerWeight[i] = 0; }
        }
    }

    void Update()
    {
        xTimer += Time.deltaTime * speed.x;
        yTimer += Time.deltaTime * speed.y;

        if (xTimer > 1) { xTimer--; }
        if (yTimer > 1) { yTimer--; }

        GetInput();

        float modX = xIn;
        float modY = yIn;

        delayInput = new Vector2(Mathf.Lerp(delayInput.x, modX, Time.deltaTime * 6f), Mathf.Lerp(delayInput.y, modY, Time.deltaTime * 6f));

        Vector3 newPos = new Vector3(0, 0, 0);
        for(int i = 0; i < layers.Count; i++)
        {
            newPos = new Vector3(Mathf.Lerp(minMaxXOffset.x, minMaxXOffset.y, delayInput.x * layerWeight[i]) + 1f, Mathf.Lerp(minMaxYOffset.x, minMaxYOffset.y, delayInput.y * layerWeight[i]) + 1f, 0);
            if (pixelPerfect) { newPos = new Vector3((Mathf.RoundToInt(newPos.x * 10f) / 10f), (Mathf.RoundToInt(newPos.y * 10f) / 10f), (Mathf.RoundToInt(newPos.z * 10f) / 10f)); }
            layers[i].transform.localPosition = newPos;
        }
    }

    void GetInput()
    {
        float mouseX = 1f - (Input.mousePosition.x / Camera.main.scaledPixelWidth);
        float mouseY = 1f - (Input.mousePosition.y / Camera.main.scaledPixelHeight);

        mouseX = Mathf.Clamp(mouseX, 0f, 1f);
        mouseY = Mathf.Clamp(mouseY, 0f, 1f);

        mouseX *= inputIntensity.x;
        mouseY *= inputIntensity.y;

        xIn = mouseX;
        yIn = mouseY;
    }
}
