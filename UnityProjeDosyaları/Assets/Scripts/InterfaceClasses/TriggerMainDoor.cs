using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TriggerMainDoor : MonoBehaviour,ISuccess
{
    [SerializeField] GameObject door;

    public void Success()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            door.GetComponent<MainDoor>().Trigger();
        }
    }
}
