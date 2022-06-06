using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public Transform FirstPlayerSpawn;
    public Transform SecondPlayerSpawn;

    private void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, FirstPlayerSpawn.position, Quaternion.identity);

        //if (PhotonNetwork.IsMasterClient)
        //{
        //    PhotonNetwork.Instantiate(playerPrefab.name, FirstPlayerSpawn.position, Quaternion.identity);
        //}
        //else
        //{
        //    PhotonNetwork.Instantiate(playerPrefab.name, SecondPlayerSpawn.position, Quaternion.identity);
        //}
    }
}
