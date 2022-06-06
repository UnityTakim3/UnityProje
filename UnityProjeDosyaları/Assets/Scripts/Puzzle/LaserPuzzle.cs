using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPuzzle : MonoBehaviour
{
    LineRenderer line;
    float rayRange;
    public float rayRangeStart = 10f;

    [SerializeField] GameObject door;
    List<GameObject> players = new List<GameObject>();
    void Start()
    {
        rayRange = rayRangeStart;
        line = GetComponent<LineRenderer>();
    }

    void Update()
    {
        SendRaycast();

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * rayRange);
    }
    private void SendRaycast()
    {
        RaycastHit rayHit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out rayHit, rayRange,-1,QueryTriggerInteraction.Ignore))
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, rayHit.point);
            print(rayHit.transform.name);
            if (rayHit.transform.GetComponentInParent<SeizableObject>()!=null)
            {
                float diff = Vector3.Distance(transform.position, rayHit.point);
                rayRange = diff+0.15f;
            }
            if (rayHit.transform.CompareTag("Player"))
            {
                door.GetComponent<PhotonView>().RPC("Open",RpcTarget.All,true, 1, 1);
            }
        }
        else
        {
            print("else");
            rayRange = rayRangeStart;
        }
    }

  
}
