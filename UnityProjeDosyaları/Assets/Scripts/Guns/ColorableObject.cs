using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorableObject : MonoBehaviour
{
    [HideInInspector] public bool isColorChanged = false;
    Color myColor;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    Renderer renderer;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

    bool changeColor = false;
    float timer = 0f;
    private void Start()
    {
        renderer = GetComponentInChildren<Renderer>();
    }
    public void ChangeMatColor(float[] rgba)
    {
        myColor.r = rgba[0];
        myColor.g = rgba[1];
        myColor.b = rgba[2];
        myColor.a = rgba[3];
        changeColor = true;
        timer = 0f;
    }
    private void Update()
    {
        if (changeColor)
        {
            renderer.material.color = Color.Lerp(renderer.material.color, myColor, Time.deltaTime);
            timer += Time.deltaTime;
            if (timer < 1f) return;
            renderer.material.color = myColor;
            changeColor = false;
        }
    }
}
