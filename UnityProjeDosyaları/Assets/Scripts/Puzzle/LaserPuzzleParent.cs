using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPuzzleParent : MonoBehaviour
{
    List<GameObject> players = new List<GameObject>();
    [SerializeField] GameObject door;


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            players.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            players.Remove(other.gameObject);
            if (players.Count < 1)
            {
                print("close");
                door.GetComponent<PhotonView>().RPC("Close", RpcTarget.All, 1, 1);
            }
        }
    }
}
