using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class OpenDoor : MonoBehaviour,ISuccess
{
    [SerializeField] int priority = 1;
    [SerializeField] int idendity = 1;
    [SerializeField] GameObject door;

    
    public void Success()
    {
        door.GetComponent<PhotonView>().RPC("Open", RpcTarget.MasterClient, priority, idendity);
    }

    
}
