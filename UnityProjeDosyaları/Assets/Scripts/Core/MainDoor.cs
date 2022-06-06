using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MainDoor : MonoBehaviour
{
    [SerializeField] int triggerPoint;
    int triggered = 0;
   public void Trigger()
    {
       triggered++;
        if (triggered>=triggerPoint)
        {
            GetComponent<PhotonView>().RPC("Open", RpcTarget.MasterClient, true, 1, 1);
        }
    }
}
