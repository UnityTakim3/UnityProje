using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ColorGun : Gun
{

    LineRenderer line;
    [SerializeField] private Texture[] textures;
    [SerializeField]
    Transform gunPivotStart, gunPivotEnd;

    [SerializeField] float changeDist = 15, throwingPower = 10f, fireRate = 0.5f;
    float nextShootTime = 0f;
    [SerializeField] GameObject colorBallPrefab;
    public Material[] MyColors;
    private int colorRange = 0;

    private Color takenColor;
    [HideInInspector] public Color currentColor;
    bool hasColor = false;

    [SerializeField] GameObject myCam;

    PhotonView photonView;
    GunController gunController;

    [SerializeField] AudioSource colorBallThrowSound;

    Animator _animator;

    [SerializeField] AudioSource ChangeColorSound;
    private void Awake()
    {
        _animator = GetComponentInParent<Animator>();
        line = GetComponent<LineRenderer>();
        currentColor = MyColors[colorRange].color;
        photonView = GetComponent<PhotonView>();
        gunController = GetComponent<GunController>();
    }


    private void OnEnable()
    {
        line.positionCount = 2;
    }

    /* void Update()
     {
         fpsCounter += Time.deltaTime;
         LineUpdate();
         SetLine();

     }

     private void LineUpdate()
     {
         if (fpsCounter >= 1f / fps)
         {
             animationStep++;
             if (animationStep == 4)
             {
                 animationStep = 0;
             }
             line.material.SetTexture("_MainTex", textures[animationStep]);
             fpsCounter = 0f;
         }
     }
     private void SetLine()
     {
         line.SetPosition(0, gunPivotStart.position);
         line.SetPosition(1, gunPivotEnd.position);
     }*/
    public override void LeftClick()
    {
        ChangeColor();
    }
    public override void RightClick()
    {
        TakeColor();
    }
    public override void MiddleMouseUp()
    {
        photonView.RPC("ColorUp", RpcTarget.All);
    }
    public override void MiddleMouseDown()
    {
        photonView.RPC("ColorDown", RpcTarget.All);
    }

    private void ChangeColor()
    {
        /* RaycastHit hit;
         Ray ray = new Ray(myCam.transform.position, myCam.transform.forward);

         if (Physics.Raycast(ray, out hit, changeDist))
         {
             ColorableObject colorable = hit.transform.GetComponent<ColorableObject>();
             if (colorable == null) return;
             colorable.isColorChanged = true;*/
        if (hasColor)
        {
            if (Time.time > nextShootTime)
            {
                nextShootTime = fireRate + Time.time;
                _animator.SetTrigger("Throw");
                GameObject colorBall = PhotonNetwork.Instantiate(colorBallPrefab.name, gunPivotEnd.position, Quaternion.identity);
                photonView.RPC("PlayShootSound", RpcTarget.All);
                colorBall.GetComponent<PhotonView>().RPC("ShootColorBall", RpcTarget.All, new float[] { currentColor.r, currentColor.g, currentColor.b, currentColor.a }, throwingPower, myCam.transform.forward);
                //colorBall.GetComponent<ColorGunBall>().ShootColorBall(new float[] { currentColor.r, currentColor.b, currentColor.a }, throwingPower, myCam.transform.forward);
                //hit.transform.GetComponent<Renderer>().material.SetColor("_Color", takenColor);
                photonView.RPC("ChangeColorRpc", RpcTarget.All);
            }

        }
        else
        {
            if (Time.time > nextShootTime)
            {
                nextShootTime = fireRate + Time.time;
                _animator.SetTrigger("Throw");
                GameObject colorBall = PhotonNetwork.Instantiate(colorBallPrefab.name, gunPivotEnd.position, Quaternion.identity);
                photonView.RPC("PlayShootSound", RpcTarget.All);
                colorBall.GetComponent<PhotonView>().RPC("ShootColorBall", RpcTarget.All, new float[] { currentColor.r, currentColor.g, currentColor.b, currentColor.a }, throwingPower, myCam.transform.forward);
            }
            //hit.transform.GetComponent<Renderer>().material.SetColor("_Color", MyColors[colorRange].color);
            //colorable.isColorChanged = true;
        }
        //}
    }
    [PunRPC]
    private void PlayShootSound()
    {
        colorBallThrowSound.Play();
    }
    [PunRPC]
    private void ChangeColorRpc()
    {
        hasColor = false;
        currentColor = MyColors[colorRange].color;
        SetGunColor();
    }

    private void TakeColor()
    {
        RaycastHit hit;
        Ray ray = new Ray(myCam.transform.position, myCam.transform.forward);
        if (Physics.Raycast(ray, out hit, changeDist) && hit.collider.CompareTag("SeizableObject"))
        {
            ColorableObject colorable = hit.transform.GetComponent<ColorableObject>();
            if (colorable == null || !colorable.isColorChanged) return;

            int photonViewId = colorable.GetComponent<PhotonView>().ViewID;

            SetTakenColor(photonViewId);
        }
    }
    private void SetTakenColor(int photonId)
    {
        takenColor = PhotonNetwork.GetPhotonView(photonId).GetComponent<Renderer>().material.color;
        currentColor = takenColor;
        hasColor = true;
        SetGunColor();
    }
    [PunRPC]
    private void ColorUp()
    {
        if (colorRange < 3)
        {
            colorRange++;
        }
        if (colorRange >= 3)
        {
            colorRange = 0;
        }
        ChangeColorSound.Play();
        hasColor = false;
        currentColor = MyColors[colorRange].color;
        SetGunColor();
    }
    [PunRPC]
    private void ColorDown()
    {
        if (colorRange >= 0)
        {
            colorRange--;
        }
        if (colorRange < 0)
        {
            colorRange = 2;
        }
        ChangeColorSound.Play();
        hasColor = false;
        currentColor = MyColors[colorRange].color;
        SetGunColor();
    }

    void SetGunColor()
    {
        gunController.ColorGunColorChange();
        //photonView.RPC("ColorGunColorChange", RpcTarget.All);
    }


    public override void KeyboardE()
    {
        if (throwingPower == 20) return;
        throwingPower++;
    }

    public override void KeyboardQ()
    {
        if (throwingPower == 0) return;
        throwingPower--;


    }
}
