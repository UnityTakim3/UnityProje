using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class LevelSet : MonoBehaviour
{
    public short level = 0;
    Transform playerMaster;
    Transform playerClient;

    private void Start()
    {
        playerMaster = transform.GetChild(0);
        playerClient = transform.GetChild(1);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<PhotonView>().RPC("SetLevelAndTransforms", RpcTarget.All);
        }
    }
    [PunRPC]
    private void SetLevelAndTransforms()
    {
        GameObject.FindWithTag("LevelController").GetComponent<LevelController>().level = level;
        GameObject.FindWithTag("PlayerSpawnMaster").transform.position = playerMaster.transform.position;
        GameObject.FindWithTag("PlayerSpawnClient").transform.position = playerClient.transform.position;
    }
}
