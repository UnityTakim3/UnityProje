using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorCombiningObject : MonoBehaviour
{
    [SerializeField] GameObject object1;
    [SerializeField] GameObject object2;

    [SerializeField] bool destroyScriptAfterSuccess = false;
    [SerializeField] bool returnBool = false;

    Material mat1;
    Material mat2;

    List<Material> mats = new List<Material>();

    [SerializeField] string alert = "you need a ISuccess Script to do something";
    public enum ColorsEnum { Yellow, Red, Blue, Empty };
    public ColorsEnum firstColor = ColorsEnum.Red;
    public ColorsEnum secondColor = ColorsEnum.Blue;
    public List<ColorsEnum> colorArray = new List<ColorsEnum>();

    

    Renderer myRenderer;
    [SerializeField] string ColorCombinings = "Yellow+Blue=Green---Yellow+Red=Orange---Red+Blue=Purple";

    private void Start()
    {
        myRenderer = GetComponentInChildren<Renderer>();
        mat1 = object1.GetComponentInChildren<Renderer>().material;
        mat2 = object2.GetComponentInChildren<Renderer>().material;
        mats.Add(mat1);
        mats.Add(mat2);
    }
    private void FixedUpdate()
    {
        CheckColors();

    }
    void CheckColors()
    {
        for (int i = 0; i < mats.Count; i++)
        {
            if (i == 0)
            {
                colorArray.Add(FindColor(mats[0]));
            }
            else
            {
                colorArray.Add(FindColor(mats[1]));

            }
        }

        if (colorArray.Contains(firstColor) && colorArray.Contains(secondColor)) Success();

        ChangeMyColor();
    }

    private void Success()
    {
        if (returnBool) return;
        
        GetComponent<ISuccess>().Success();
        if (destroyScriptAfterSuccess) 
        returnBool = true;
    }
    void ChangeMyColor()
    {

        if (colorArray.Contains(ColorsEnum.Red) && colorArray.Contains(ColorsEnum.Blue))
        {
            myRenderer.material.color = Color.Lerp(myRenderer.material.color, new Color32(143, 0, 254, 1), Time.deltaTime);
            colorArray.Clear();
        }
        if (colorArray.Contains(ColorsEnum.Red) && colorArray.Contains(ColorsEnum.Yellow))
        {
            myRenderer.material.color = Color.Lerp(myRenderer.material.color, new Color32(254, 161, 0, 1), Time.deltaTime);
            colorArray.Clear();
        }
        if (colorArray.Contains(ColorsEnum.Yellow) && colorArray.Contains(ColorsEnum.Blue))
        {
            myRenderer.material.color = Color.Lerp(myRenderer.material.color, new Color32(0, 254, 111, 1), Time.deltaTime);
            colorArray.Clear();
        }
        else
        {
            colorArray.Clear();
        }

    }

    ColorsEnum FindColor(Material mat)
    {
        if (mat.color.r == 0 && mat.color.g == 0 && mat.color.b == 1 && mat.color.a == 1)
        {
            return ColorsEnum.Blue;
        }
        else if (mat.color.r == 1 && mat.color.g == 0 && mat.color.b == 0 && mat.color.a == 1)
        {
            return ColorsEnum.Red;
        }
        else if (mat.color.r == 1 && mat.color.g == 1 && mat.color.b == 0 && mat.color.a == 1)
        {
            return ColorsEnum.Yellow;
        }
        else
        {
            return ColorsEnum.Empty;
        }
    }
}
