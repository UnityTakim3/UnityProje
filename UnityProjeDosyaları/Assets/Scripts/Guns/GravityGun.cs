    using System.Collections;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using DigitalRuby.LightningBolt;
public class GravityGun : Gun
{
    public PhotonView[] views;
    SeizableObject lastTarget = null;

    [SerializeField] Transform gunPivotStart, gunPivotEnd, objectPivot;
    bool bring = false;

    [SerializeField] private Texture[] textures;

    [SerializeField]
    float grabSpeed, throwingPower, robotThrowingPowerMultiplier, grabDistance;
    [SerializeField] ParticleSystem lightning;
    [SerializeField] TextMeshProUGUI powerText;
    float changePivotPosition = 0;
    LineRenderer line;
    Material lineMaterial;
    PhotonView photonView;
    [SerializeField] GameObject myFace;

    [SerializeField] AudioSource electricSound;
    [SerializeField] AudioSource ThrowSound;

    
    
    Animator _animator;
    LightningBoltScript boltScript;
    private void Awake()
    {
        boltScript = GetComponent<LightningBoltScript>();
        _animator = GetComponentInParent<Animator>();
        photonView = GetComponent<PhotonView>();
        line = GetComponent<LineRenderer>();
        lineMaterial = line.material;

        line.positionCount = 2;

    }
    private void OnEnable()
    {
        line.positionCount = 2;
    }
    private void OnDisable()
    {
        ReleaseLastTarget();
    }

    private void FixedUpdate()
    {
        //fpsCounter += Time.deltaTime;
        //LineUpdate();
        if (lastTarget != null)
        {

            float distance = Vector3.Distance(objectPivot.transform.position, lastTarget.transform.position);
            if (distance > grabDistance)
            {
                if (photonView.IsMine)
                {
                    ReleaseLastTarget();
                }
                lastTarget = null;
                return;

            }

            SetLine(lastTarget.gameObject);

            BringTarget(lastTarget.GetComponent<SeizableObject>());

            PlaySound(true, "bring");
            return;
        }
        if (photonView.IsMine)
        {
            LookForSeizableObjects();
        }
        SetLine(gunPivotEnd.gameObject);

    }
    private void SetLine(GameObject third)
    {
        boltScript.EndObject = third;
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
            lineMaterial.SetTexture("_MainTex", textures[animationStep]);
            //GetComponent<Renderer>().material.SetTexture("_MainTex", textures[animationStep]);
            fpsCounter = 0f;
        }
    }

    private void LookForSeizableObjects()
    {
        RaycastHit hit;
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
        if (Physics.Raycast(ray, out hit, grabDistance, -1, QueryTriggerInteraction.Ignore))
        {
            SeizableObject target = hit.transform.GetComponent<SeizableObject>();
            if (target == null || !bring || target.isThrown) return;
            _animator.ResetTrigger("Throw");
            _animator.SetBool("Bring", true);
            int photonViewId = target.GetComponent<PhotonView>().ViewID;
            print("hop");
            photonView.RPC("SetLastTarget", RpcTarget.All, photonViewId);
        }

    }
    [PunRPC]
    private void SetLastTarget(int photonView)
    {
        lastTarget = PhotonNetwork.GetPhotonView(photonView).GetComponent<SeizableObject>();
    }
    private void ReleaseLastTarget()
    {
        if (lastTarget == null) return;
        _animator.SetBool("Bring", false);
        //lastTarget.GetComponent<SeizableObject>().Release(true);
        photonView.RPC("SetTargetNull", RpcTarget.All, true);

    }
    private void ReleaseLastTarget(bool wannaRelease)
    {
        if (lastTarget == null) return;
        //lastTarget.GetComponent<SeizableObject>().Release(true);
        photonView.RPC("SetTargetNull", RpcTarget.All, false);

    }
    [PunRPC]
    private void SetTargetNull(bool wannaRelease)
    {
        if (wannaRelease)
        {
            lastTarget.Release(true);
        }
        lastTarget = null;
        PlaySound(false, "bring");
    }

    private void BringTarget(SeizableObject target)
    {
        if (photonView.IsMine&&!target.GetComponent<PhotonView>().AmOwner)
        {
            print("AmOwner");
            target.GetComponent<PhotonView>().RequestOwnership();
        }
        
        target.Bring(objectPivot.position, grabSpeed, myFace.transform.forward);
        //target.GetComponent<PhotonView>().RPC("Bring", RpcTarget.All, objectPivot.position, grabSpeed, myFace.transform.forward);

        /*views = target.transform.root.GetComponentsInChildren<PhotonView>();
        for (int i = 0; i < views.Length; i++)
        {
            views[i].RequestOwnership();
        }*/



        //target.GetComponent<PhotonView>().RequestOwnership();
        //target.Bring(objectPivot.position, grabSpeed, myFace.transform.forward);
    }

    private void ThrowTarget()
    {

        if (lastTarget == null) return;
        if (lastTarget.GetComponent<SeizableObject>().CanThrow())
        {
            _animator.SetTrigger("Throw");
            _animator.SetBool("Bring", false);
            lastTarget.GetComponent<PhotonView>().RPC("Throw", RpcTarget.All, throwingPower, robotThrowingPowerMultiplier, myFace.transform.forward);
            ReleaseLastTarget(false);
            //lastTarget.GetComponent<SeizableObject>().Throw(throwingPower, myFace.transform.forward);
            GameObject createdEffect = PhotonNetwork.Instantiate(lightning.name, gunPivotEnd.position, gunPivotEnd.rotation);
            //lightning.Play();
            lastTarget = null;
            photonView.RPC("PlaySound", RpcTarget.All, true, "throw");
        }

    }
    [PunRPC]
    private void PlaySound(bool playOrStop, string voice)
    {
        if (playOrStop)
        {
            switch (voice)
            {
                case "bring":
                    if (!electricSound.isPlaying)
                    {
                        electricSound.Play();
                        electricSound.loop = true;
                    }
                    break;
                case "throw":
                    
                        ThrowSound.Play();
                    
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (voice)
            {
                case "bring":

                    electricSound.Stop();
                    electricSound.loop = false;

                    break;
                case "throw":

                    ThrowSound.Stop();

                    break;
                default:
                    break;
            }
        }

    }
    public override void LeftClick()
    {
        //photonView.RPC("ThrowTarget", RpcTarget.All);
        ThrowTarget();
    }

    public override void RightClick()
    {
        bring = true;
    }
    public override void RightClickOff()
    {
        bring = false;
        _animator.SetBool("Bring", false);
        ReleaseLastTarget();

    }

    public override void KeyboardE()
    {
        if (throwingPower == 20) return;
        throwingPower++;
        powerText.text = throwingPower.ToString();
    }

    public override void KeyboardQ()
    {
        if (throwingPower == 0) return;
        throwingPower--;
        powerText.text = throwingPower.ToString();


    }
    public override void MiddleMouseUp()
    {
        if (changePivotPosition == 10) return;
        Vector3 direction = (objectPivot.position - myFace.transform.position).normalized;
        photonView.RPC("ChangeObjectPivot", RpcTarget.All, direction, true);
        changePivotPosition++;
    }

    public override void MiddleMouseDown()
    {
        if (changePivotPosition == 0) return;
        Vector3 direction = (objectPivot.position - myFace.transform.position).normalized;
        photonView.RPC("ChangeObjectPivot", RpcTarget.All, direction, false);
        changePivotPosition--;

    }
    [PunRPC]
    private void ChangeObjectPivot(Vector3 direction, bool positiveOrNegative)
    {
        if (positiveOrNegative)
        {
            objectPivot.position += direction / 5;

        }
        else
        {
            objectPivot.position += -direction / 5;
        }
    }
}
