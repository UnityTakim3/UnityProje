using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillInstance : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PhotonView>().RPC("Die", RpcTarget.All);
        }
    }
}
