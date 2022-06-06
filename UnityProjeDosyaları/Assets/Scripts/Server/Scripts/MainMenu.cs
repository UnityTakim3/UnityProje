using UnityEngine;
using Photon.Pun;
using System.Collections;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class MainMenu : MonoBehaviourPunCallbacks
{
    public void DisconnectPlayer()
    {
        PhotonNetwork.LeaveRoom();

    }

   
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        PhotonNetwork.Disconnect();
        Application.Quit();
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Application.Quit();
    }
}