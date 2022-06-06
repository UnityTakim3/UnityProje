using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ColorGunBall : MonoBehaviour
{
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    Renderer renderer;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    Rigidbody rg;
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
    BoxCollider collider;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
    Color myColor;
    PhotonView photonView;
    bool hit = false;
    AudioSource hitSound;


    void Awake()
    {
        rg = GetComponent<Rigidbody>();
        renderer = transform.GetChild(0).GetComponent<Renderer>();
        collider = GetComponent<BoxCollider>();
        photonView = GetComponent<PhotonView>();
        hitSound = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        ColorableObject colorable = other.gameObject.GetComponentInParent<ColorableObject>();
        if (colorable == null||hit) return;
        int hitViewId = colorable.GetComponent<PhotonView>().ViewID;
        photonView.RPC("Hit", RpcTarget.All, hitViewId);
    }

    [PunRPC]
    private void Hit(int photonId)
    {
        if (hit) return;
        ColorableObject colorable = PhotonNetwork.GetPhotonView(photonId).gameObject.GetComponent<ColorableObject>();
        colorable.isColorChanged = true;
        colorable.ChangeMatColor(new float[] { myColor.r, myColor.g, myColor.b, myColor.a });
        hit = true;
        hitSound.Play();
        Destroy(gameObject,1.5f);

        //colorable.transform.GetComponent<Renderer>().material.SetColor("_Color", myColor);

    }


    [PunRPC]
    public void ShootColorBall(float[] rgb, float throwingPower, Vector3 cam)
    {

        myColor.r = rgb[0];
        myColor.g = rgb[1];
        myColor.b = rgb[2];
        myColor.a = rgb[3];
        renderer.material.SetColor("_TintColor", myColor);
        rg.AddForce(cam * throwingPower, ForceMode.Impulse);

    }
    /*    RaycastHit hit;
        Ray ray = new Ray(myCam.transform.position, myCam.transform.forward);

            if (Physics.Raycast(ray, out hit, changeDist))
            {
                ColorableObject colorable = hit.transform.GetComponent<ColorableObject>();
                if (colorable == null) return;
                colorable.isColorChanged = true;
                if (hasColor)
                {
                    hit.transform.GetComponent<Renderer>().material.SetColor("_Color", takenColor);
                    hasColor = false;
                    currentColor = MyColors[colorRange].color;
                    SetGunColor();
    }
                else
    {
        hit.transform.GetComponent<Renderer>().material.SetColor("_Color", MyColors[colorRange].color);
        colorable.isColorChanged = true;
    }
            }*/
}
