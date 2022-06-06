using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : Gun
{

    [SerializeField] float changeDist = 15;
    private Material hitObjectMaterial;
    private Collider hitCollider;
    public Material[] hitColor;
    private int colorRange = 0;
    private bool colorMode = false;
    private bool isHit = false;

    public void Start()
    {
        hitObjectMaterial = GetComponent<Material>();
        hitCollider = GetComponent<Collider>();


    }

    /*public override void SwitchColor()//PRESS F//
    {

        colorMode = true;

    }*/
    public override void MiddleMouseUp()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);


        if (Physics.Raycast(ray, out hit, changeDist) && isHit == true && hit.collider.CompareTag("SeizableObject"))
        {

            if (colorRange < 3)
            {
                colorRange++;
            }

            if (colorRange >= 3)
            {
                colorRange = 0;
            }

            hitObjectMaterial.color = hitColor[colorRange].color;
            Debug.Log("MouseUp" + " " + colorRange);
        }
    }
    public override void MiddleMouseDown()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, changeDist) && isHit == true && hit.collider.CompareTag("SeizableObject"))
        {
            if (colorRange >= 0)
            {
                colorRange--;

            }
            if (colorRange < 0)
            {
                colorRange = 2;
            }
            hitObjectMaterial.color = hitColor[colorRange].color;
            Debug.Log("MouseDown" + " " + colorRange);
        }
    }
    public override void RightClick() //Store Color//
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, changeDist) && hit.collider.CompareTag("SeizableObject"))
        {
            hitObjectMaterial = hitCollider.GetComponent<MeshRenderer>().material;
        }
    }
    public override void LeftClick() // Shoot Color//
    {

        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        isHit = true;
        if (Physics.Raycast(ray, out hit, changeDist) && hit.collider.CompareTag("SeizableObject"))
        {
            hitCollider = hit.collider;

            hitObjectMaterial = hitCollider.GetComponent<MeshRenderer>().material;

            hitObjectMaterial.color = hitColor[colorRange].color;
        }
    }

}


















































































