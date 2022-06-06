using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class FinishGame : MonoBehaviourPunCallbacks
{
    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GetComponent<PhotonView>().RPC("FinishTheGame", RpcTarget.All);
        }
    }
    [PunRPC]
    public void FinishTheGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SceneManager.LoadScene("Credit");
        }
    }
}
