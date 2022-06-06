using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class CreateRoomOrJoin : MonoBehaviourPunCallbacks
{
    [SerializeField] InputField createRoomInput;
    [SerializeField] InputField createRoomPassword;
    [SerializeField] InputField joinRoomPassword;
    [SerializeField] Text roomName;
    [SerializeField] GameObject lobbyPanel, RoomPanel, passwordPanel;
    [SerializeField] Button createRoomBtn;

    public RoomItem roomItemPrefab;
    public Transform contentObject;

    public List<Text> players = new List<Text>();
    public List<RoomItem> roomItemsList = new List<RoomItem>();


    [SerializeField] Text playerPrefab;
    [SerializeField] GameObject playerNamesObject;

    [SerializeField] float refleshRooms = 1.5f;
    float nextRefleshRooms;

    [SerializeField] Button startGameBtn;


    string _password;
    string _roomName;

    public const string ROOM_PASSWORD = "rp";

   
    private void Start()
    {
        PhotonNetwork.JoinLobby();
        createRoomBtn.interactable = false;
        startGameBtn.interactable = false;
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void FixedUpdate()
    {

        if (createRoomInput.text.Length < 3 && createRoomPassword.text.Length < 3)
        {
            createRoomBtn.interactable = false;
        }
        else
        {
            createRoomBtn.interactable = true;
        }


        if (players.Count >= 2 && PhotonNetwork.IsMasterClient)
        {
            startGameBtn.transform.GetChild(0).GetComponent<Text>().text = "Start game";
            startGameBtn.interactable = true;
        }
        else if (players.Count >= 2)
        {
            startGameBtn.transform.GetChild(0).GetComponent<Text>().text = "waiting for host";
            startGameBtn.interactable = false;
        }
        else
        {
            startGameBtn.transform.GetChild(0).GetComponent<Text>().text = "waiting for Second player";

        }



    }
    public void CreateRoom()
    {

        /*
        var roomProperties = new ExitGames.Client.Photon.Hashtable();
        roomProperties.Add("password", createRoomPassword.text);

        string[] lobbyProperties = new string[1];            //Property'leri lobby'ye göndermiyor
        lobbyProperties[0] = "password";
        
        options.CustomRoomPropertiesForLobby = lobbyProperties;
        options.CustomRoomProperties = roomProperties;
        */
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        string[] lobbyProperties = { ROOM_PASSWORD };

        roomOptions.CustomRoomPropertiesForLobby = lobbyProperties;
        roomOptions.CustomRoomProperties = new Hashtable { { ROOM_PASSWORD, createRoomPassword.text } };

        //print(roomOptions.CustomRoomProperties[ROOM_PASSWORD]);

        PhotonNetwork.CreateRoom(createRoomInput.text, roomOptions, null);
    }
    public void PasswordBeforeJoinRoom(string password, string roomName)
    {
        lobbyPanel.SetActive(false);
        RoomPanel.SetActive(false);
        passwordPanel.SetActive(true);
        _password = password;
        _roomName = roomName;
    }
    public void ExitPasswordPanel()
    {
        lobbyPanel.SetActive(true);
        RoomPanel.SetActive(false);
        passwordPanel.SetActive(false);
    }
    public void JoinRoom()
    {
        if (joinRoomPassword.text == _password)
        {
            PhotonNetwork.JoinRoom(_roomName);

        }
        else
        {
            joinRoomPassword.placeholder.GetComponent<Text>().text = "Wrong password";
            joinRoomPassword.text = "";
        }
        //photonView.RPC("setTheRoomPlayers", RpcTarget.All);

    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        //photonView.RPC("setTheRoomPlayers", RpcTarget.All);

    }
    public override void OnJoinedRoom()
    {
        lobbyPanel.SetActive(false);
        RoomPanel.SetActive(true);
        passwordPanel.SetActive(false);

        roomName.text = "Room Name: " + PhotonNetwork.CurrentRoom.Name;
        //photonView.RPC("setTheRoomPlayers", RpcTarget.All);
        setTheRoomPlayers(true);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        setTheRoomPlayers(true);
    }
    public override void OnLeftRoom()
    {

        RoomPanel.SetActive(false);
        lobbyPanel.SetActive(true);
        passwordPanel.SetActive(false);
        setTheRoomPlayers(false);
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        setTheRoomPlayers(true);

        //photonView.RPC("setTheRoomPlayers", RpcTarget.All);
    }

    void setTheRoomPlayers(bool hasRoom)
    {
        for (int i = 0; i < players.Count; i++)
        {
            Destroy(players[i].gameObject);
        }
        players.Clear();

        if (!hasRoom) return;
        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
            //if (PhotonNetwork.PlayerList.Length < 1) return
            Text player = Instantiate(playerPrefab, playerNamesObject.transform);
            player.text = PhotonNetwork.PlayerList[i].NickName;
            players.Add(player);
        }
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        
        if (Time.time >= nextRefleshRooms)
        {
            UpdateRoomList(roomList);
            nextRefleshRooms = Time.time + refleshRooms;
        }

    }
    private void UpdateRoomList(List<RoomInfo> roomList)
    {

        foreach (RoomInfo room in roomList)
        {
            if (room.RemovedFromList)
            {
                for (int i = 0; i < roomItemsList.Count; i++)
                {
                    if (roomItemsList[i].roomName.text == room.Name)
                    {
                        Destroy(roomItemsList[i].gameObject);
                        roomItemsList.RemoveAt(i);
                    }
                }
            }
            if (!room.RemovedFromList)
            {
                bool skip = false;
                for (int i = 0; i < roomItemsList.Count; i++)
                {
                    if (roomItemsList[i].roomName.text == room.Name)
                    {
                        skip = true;
                    }
                }
                if (!skip)
                {
                    RoomItem newRoom = Instantiate(roomItemPrefab, contentObject);
                    newRoom.SetRoomName(room.Name);
                    newRoom.SetRoomPass(room.CustomProperties[ROOM_PASSWORD].ToString());
                    //print(room.CustomProperties[ROOM_PASSWORD]);
                    roomItemsList.Add(newRoom);
                    if (room.PlayerCount == 0)
                    {
                        Destroy(newRoom.gameObject);
                        roomItemsList.Remove(newRoom);
                    }
                }
            }
        }
    }



    public void StartGame()
    {

        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
            PhotonNetwork.CurrentRoom.IsVisible = true;
            PhotonNetwork.LoadLevel(2);
        }
    }

}
