using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


[RequireComponent(typeof(Rigidbody))]
public class SeizableObject : MonoBehaviour
{
    Rigidbody _rb;
    public bool isThrown = false;
    bool onHand = false;
    bool fixedBool = false;
    bool seconFixedBool = false;
    Coroutine collisionCoroutine;
    MeshRenderer meshRenderer;
    Color startColor;
    float deltaTime = 1f;
    float transparencyFloat = 0.65f;
    public bool isRobot = false;
    PhotonView photonView;
    bool syncHasChange = false;
    private void Awake()
    {
        photonView = GetComponentInParent<PhotonView>();
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        photonView.OwnershipTransfer = OwnershipOption.Takeover;
        _rb.interpolation = RigidbodyInterpolation.Extrapolate;
        _rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        try
        {
            meshRenderer = GetComponent<MeshRenderer>();
            startColor = meshRenderer.material.color;
        }
        catch (Exception) { }


    }

    public void Bring(Vector3 pivot, float grabSpeed, Vector3 face)
    {
        if (!syncHasChange)
        {
            photonView.Synchronization = ViewSynchronization.Unreliable;
            syncHasChange = true;
        }
        if (isThrown) return;
        Fix();
        //Fix();
        Vector3 position = pivot;
        Vector3 direction = (position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, position);
        float multiplicationDistance = distance * 20;
        Vector3 rbVelocity = direction * grabSpeed * multiplicationDistance * Time.deltaTime;
        if (isRobot&&distance<1)
        {
            _rb.velocity = rbVelocity * 3.5f;
        }
        else
        {
            _rb.velocity = rbVelocity;
        }

        if (distance < 1)
        {
            SecondFix();
            //SecondFix();
            //GetComponent<PhotonView>().RPC("ChangeTransparency", RpcTarget.All); // Built in'de color.a(alpha) çalýþmýyor
            deltaTime -= Time.deltaTime / 3;

            //_rb.MovePosition(transform.position + direction * 1 * Time.deltaTime);  // rigidbody'i kinematic olarak hareket ettirdiði için bug'a sebep olabiliyor
            //_rb.AddForce(direction *0.01f * Time.deltaTime); // bozuk
            //_rb.velocity = direction * grabSpeed  * Time.deltaTime;
            _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(face), 150f * Time.deltaTime));
            return;
        }
        //_rb.MovePosition(transform.position + direction * grabSpeed * Time.deltaTime);  // rigidbody'i kinematic olarak hareket ettirdiði için bug'a sebep olabiliyor

        //_rb.MoveRotation(Quaternion.LookRotation(Camera.main.transform.forward * 0.1f * Time.deltaTime));
    }
    /*
    private void ChangeTransparency()
    {
        if (!onHand) return;
        if (deltaTime <= transparencyFloat)
        {
            deltaTime = transparencyFloat;
        }
        Color color = startColor;
        color = Color.blue;
        color.a = deltaTime;
        //color.a = transparencyFloat;
        //Material mat = new Material(Shader.Find("Standard"));
        Material mat = meshRenderer.material;
        mat.SetColor("_Color", color);
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 3000;
        meshRenderer.material = mat;
    }*/

    private void FreeFix()
    {
        _rb.isKinematic = true;
        _rb.isKinematic = false;
    }
    private void Fix()
    {
        if (fixedBool) return;
        _rb.useGravity = false;

        _rb.isKinematic = true;

        _rb.isKinematic = false;
        fixedBool = true;
    }

    private void SecondFix()
    {
        onHand = true;

        if (seconFixedBool) return;
        _rb.isKinematic = true;

        _rb.isKinematic = false;
        seconFixedBool = true;
    }
    [PunRPC]
    public void Release(bool wannaFix)
    {
        if (wannaFix)
        {
            FreeFix();
        }
        //photonView.TransferOwnership(PhotonNetwork.MasterClient);
        onHand = false;
        _rb.useGravity = true;
        fixedBool = false;
        seconFixedBool = false;
        //meshRenderer.material.SetColor("_Color", startColor);
        deltaTime = 1f;
    }
    [PunRPC]
    public void Throw(float throwingPower, float multiplier, Vector3 face)
    {
        Release(true);
        if (isRobot)
        {
            Health health = GetComponentInParent<Health>();
            //StartCoroutine(SetPhotonTransform(health));
            health.EnablePhotonComps();
            GameObject spine = health.pivot;
            spine.GetComponent<Rigidbody>().AddForce(face * throwingPower * multiplier, ForceMode.Impulse);
        }
        else
        {
            _rb.AddForce(face * throwingPower, ForceMode.Impulse);
        }
        if (isRobot)
        {
            StartCoroutine(IsThrown(2, true));
        }
        else
        {
            StartCoroutine(IsThrown(2, false));
        }
    }
    IEnumerator SetPhotonTransform(Health health)
    {
        health.EnablePhotonComps();
        yield return new WaitForSeconds(5f);
        health.DisablePhotonComps();

    }
    public void OwnershipRequest()
    {
        if (isRobot) return;
        photonView.RequestOwnership();
    }
    public bool CanThrow()
    {
        if (!onHand) return false;
        return true;
    }
    IEnumerator IsThrown(float seconds, bool isRobot)
    {
        if (isRobot)
        {
            GetComponentInParent<Health>().SeizablesIsThrown(true);
        }
        else
        {
            isThrown = true;
        }
        yield return new WaitForSeconds(seconds);
        if (isRobot)
        {
            GetComponentInParent<Health>().SeizablesIsThrown(false);
        }
        else
        {
            isThrown = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            if (collisionCoroutine != null)
            {
                StopCoroutine(collisionCoroutine);
            }
            if (isRobot)
            {
                collisionCoroutine = StartCoroutine(IsThrown(1f, true));

            }
            else
            {
                collisionCoroutine = StartCoroutine(IsThrown(1f, false));
            }

            Release(false);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        SeizableObject seizable = collision.gameObject.GetComponent<SeizableObject>();
        if (!isRobot && seizable != null)
        {
            //photonView.RPC("ChangePunSync", RpcTarget.All, true);
        }

        if (!onHand) return;
        FreeFix();
        //Release();
        //if (collisionCoroutine != null)
        //{
        //    StopCoroutine(collisionCoroutine);
        //}
        //collisionCoroutine = StartCoroutine(IsThrown(3f));
    }
    private void OnCollisionEnter(Collision collision)
    {
        SeizableObject seizable = collision.gameObject.GetComponent<SeizableObject>();
        if (!isRobot&&seizable!=null)
        {
            if (photonView.Owner!=PhotonNetwork.MasterClient)
            {
                print("ownerMaster");
            photonView.TransferOwnership(PhotonNetwork.MasterClient);
            }

            //photonView.RPC("ChangePunSync", RpcTarget.All, false);
        }
    }
    [PunRPC]
    private void ChangePunSync(bool isUnreliableOnChange)
    {
        print("changePunSync");
        return;
        if (isUnreliableOnChange)
        {
            photonView.Synchronization = ViewSynchronization.UnreliableOnChange;
        }
        else
        {
            photonView.Synchronization = ViewSynchronization.Unreliable;

        }
    }
}
