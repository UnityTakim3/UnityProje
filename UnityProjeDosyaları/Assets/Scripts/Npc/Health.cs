using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Health : MonoBehaviour
{
    public bool isAlive = true;
    public GameObject pivot;
    [HideInInspector] public Rigidbody[] rigids;
    [HideInInspector] public PhotonTransformView[] transformViews;
    [HideInInspector] public PhotonRigidbodyView[] rigidbodyViews;
    PhotonView photonView;
    [SerializeField] GameObject weapon;
    List<SeizableObject> seizables = new List<SeizableObject>();

    GameObject[] level1AllNpc;
    private void Start()
    {
        level1AllNpc = GameObject.FindGameObjectsWithTag("NpcLevel1");
        rigids = GetComponentsInChildren<Rigidbody>();
        //transformViews = GetComponentsInChildren<PhotonTransformView>();
        rigidbodyViews = GetComponentsInChildren<PhotonRigidbodyView>();
        photonView = GetComponent<PhotonView>();


    }
    private void OnTriggerEnter(Collider other)
    {

        if (!isAlive) return;
        SeizableObject seizable = other.transform.GetComponentInParent<SeizableObject>();

        if (seizable != null&& seizable.isThrown)
        {
            //Fall();
            DropWeapon();
            photonView.RPC("Fall", RpcTarget.All);
        }
    }

    private void DropWeapon()
    {
        weapon.transform.parent = null;
        weapon.GetComponent<Rigidbody>().isKinematic = false;
        //weapon.GetComponent<PhotonTransformView>().enabled = true;
        weapon.GetComponent<PhotonRigidbodyView>().enabled = true;
        weapon.AddComponent<SeizableObject>();
    }

    [PunRPC]
    private void Fall()
    {
        if (!isAlive) return;
        isAlive = false;
        GetComponent<ActionScheduler>().CancelCurrentAction();

        GetComponent<PhotonTransformView>().enabled = false;
        foreach (GameObject item in level1AllNpc)
        {
            NpcRobotController controller = item.GetComponent<NpcRobotController>();
            if (controller == null) continue;
            controller.sightDistance = 999f;
            
        }

        Destroy(GetComponent<NpcRobotController>());
        Destroy(GetComponent<Mover>());
        Destroy(GetComponent<Fighter>());
        Destroy(GetComponent<NavMeshAgent>());
        Destroy(GetComponent<Collider>());
        Destroy(transform.GetChild(0).GetComponent<Collider>());
        Destroy(GetComponent<ActionScheduler>());


        GetComponentInChildren<Animator>().enabled = false;

        AddSeizableScript();
        EnablePhotonComps();
        //Vector3 direction = (transform.position - seizable.transform.position).normalized;
        //GetComponent<Rigidbody>().AddForce(-direction * 5f, ForceMode.Force);
    }

    private void AddSeizableScript()
    {
        foreach (Rigidbody item in rigids)
        {
            if (item.transform.parent == null) continue;
            item.isKinematic = false;
            SeizableObject seizable = item.gameObject.AddComponent<SeizableObject>();
            seizable.isRobot = true;
            seizables.Add(seizable);
        }
    }
    public void SeizablesIsThrown(bool isThrown)
    {
        if (isThrown)
        {
            foreach (var item in seizables)
            {
                item.isThrown = true;
            }
        }
        else
        {
            foreach(var item in seizables)
            {
                item.isThrown = false;
            }
        }
        
    }
    public void EnablePhotonComps()
    {
        //foreach (PhotonTransformView item in transformViews)
        //{
        //    item.enabled = true;
        //}
        foreach (PhotonRigidbodyView item in rigidbodyViews)
        {
            item.enabled = true;

        }
    }
    public void DisablePhotonComps()
    {
        //foreach (PhotonTransformView item in transformViews)
        //{
        //    item.enabled = false;

        //}
        foreach (PhotonRigidbodyView item in rigidbodyViews)
        {
            item.enabled = false;
        }
    }
}

