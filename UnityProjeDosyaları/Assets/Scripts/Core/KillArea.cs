using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PhotonView>().RPC("Die", RpcTarget.All);
        }
    }
}
