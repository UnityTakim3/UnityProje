using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GunController : MonoBehaviour
{
    [SerializeField] ParticleSystem[] plasmaElectricEffect;
    [SerializeField] ParticleSystem ColorBall;
    [SerializeField] Material plasmaFireEffect;
    [SerializeField] Gun gravityGun, colorGun;
    Color startColor;
    [SerializeField] ParticleSystem[] lights;
    LineRenderer lineRenderer;
    bool hasChanged = false;
    [SerializeField] AudioSource gunChangeSound;

    Color currentColor;
    PhotonView photonView;
    Animator _animator;
    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
        photonView = GetComponent<PhotonView>();
        lineRenderer = GetComponent<LineRenderer>();
        startColor = Color.white;
        photonView.RPC("FixColorSettings", RpcTarget.All);
    }
    public void ChangeGun()
    {
        _animator.SetBool("ChangeGun", true);   
    }
    public void GunChanged()
    {
        photonView.RPC("ChangeGunScripts", RpcTarget.All);
        photonView.RPC("FixColorSettings", RpcTarget.All);
    }

    [PunRPC]
    private void ChangeGunScripts()
    {

        if (gravityGun.enabled == true)
        {
            gravityGun.enabled = false;
        }
        else
        {
            gravityGun.enabled = true;

        }
        if (colorGun.enabled == true)
        {
            colorGun.enabled = false;

        }
        else
        {
            colorGun.enabled = true;
        }
        gunChangeSound.Play();

    }
    [PunRPC]
    private void FixColorSettings()
    {
        if (colorGun.enabled == true)
        {
            currentColor = GetComponent<ColorGun>().currentColor;
            GunEffectSettings(true);
            ColorSettings(currentColor);
        }
        else
        {
            GunEffectSettings(false);
            ColorSettings(startColor);
        }
    }

    private void GunEffectSettings(bool isColorGun)
    {
        if (isColorGun)
        {
            plasmaElectricEffect[0].transform.parent.gameObject.SetActive(false);
            ColorBall.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            plasmaElectricEffect[0].transform.parent.gameObject.SetActive(true);
            ColorBall.transform.parent.gameObject.SetActive(false);
        }
    }
    private void ColorSettings(Color color)
    {

        plasmaFireEffect.SetColor("_TintColor", color);
        GetComponent<Renderer>().material.SetColor("_Color", color);

        foreach (ParticleSystem item in plasmaElectricEffect)
        {
            item.GetComponent<Renderer>().material.SetColor("_TintColor", color);
            item.GetComponent<ParticleSystemRenderer>().trailMaterial = item.GetComponent<Renderer>().material;
        }
        ColorBall.GetComponent<Renderer>().material.SetColor("_TintColor", color);

        foreach (var item in lights)
        {
            var col = item.colorOverLifetime;
            col.color = color;
        }
    }
    [PunRPC]
    public void ColorGunColorChange()
    {
        currentColor = GetComponent<ColorGun>().currentColor;
        ColorSettings(currentColor);
    }

}
