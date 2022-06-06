using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class ConnectToServer : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField playerName;
    [SerializeField] Button connectButton,connectDeveloperButton;
    [SerializeField] Text buttonText;
    bool isConnected = false;

    bool developerConnect = false;
    void Start()
    {
        buttonText.text = "Waiting for server";
    }
    private void FixedUpdate()
    {
        if (isConnected)
        {
            connectButton.interactable = false;
            return;
        }
        if (playerName.text.Length >= 3)
        {
            connectButton.interactable = true;
            buttonText.text = "Connect";
        }
        else
        {
            connectButton.interactable = false;
            buttonText.text = "Need player name";

        }
    }

    public override void OnConnectedToMaster()
    {
        print("ConnectedToServer");

        if (developerConnect)
        {
            PhotonNetwork.JoinLobby();
            return;
        }
        SceneManager.LoadScene(1);

    }
    public void Connect()
    {
        PhotonNetwork.NickName = playerName.text;
        buttonText.text = "Connecting";
        isConnected = true;
        //PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
        //PhotonNetwork.ConnectToRegion("us");
        PhotonNetwork.ConnectUsingSettings();
    }
    public void DeveloperConnect()
    {
        developerConnect = true;
        connectDeveloperButton.transform.GetChild(0).GetComponent<Text>().text = "Connecting";
        //PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
        //PhotonNetwork.ConnectToRegion("us");
        PhotonNetwork.ConnectUsingSettings();

    }
    bool developerConnect1 = false;
    public void DeveloperConnect1()
    {
        developerConnect = true;
        developerConnect1 = true;
        connectDeveloperButton.transform.GetChild(0).GetComponent<Text>().text = "Connecting";
        //PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
        //PhotonNetwork.ConnectToRegion("us");
        PhotonNetwork.ConnectUsingSettings();

    }

    public override void OnJoinedLobby()
    {
        print("JoinedToLobby");
        if (developerConnect1)
        {
            PhotonNetwork.CreateRoom("dev1");
            return;
        }
        PhotonNetwork.CreateRoom("dev");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        print("CreateRoomFailed");
        print(returnCode);
        print(message);

    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        print("JoinRoomFailed");
        print(returnCode);
        print(message);

    }
    public override void OnCreatedRoom()
    {
        print("RoomCreated");
        PhotonNetwork.LoadLevel(2);
    }
}
