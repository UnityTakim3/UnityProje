using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class Launcher : MonoBehaviourPunCallbacks
{
    public static Launcher Instance;

    [Header("TMP")]
    [SerializeField] TMP_InputField createRoomName;
    [SerializeField] TMP_InputField createRoomPassword;
    [SerializeField] TMP_InputField joinRoomPassword;
    [SerializeField] TMP_Text errorText;
    [SerializeField] TMP_Text roomNameText;

    [Header("GameObject")]
    [SerializeField] GameObject playerListItemPrefab;
    [SerializeField] GameObject startGameButton;

    [Header("Transform")]
    [SerializeField] Transform playerListContent;
    [SerializeField] Transform roomListContent;

    [SerializeField] RoomItemManager roomItemPrefab;

    public float timeBetweenUptades = 1.5f;
    public float nextUpdateTime;

    private List<RoomInfo> roomInfos;
    public List<RoomItemManager> roomItemManagers = new List<RoomItemManager>();

    string _password;
    string _roomName;

    public const string ROOM_PASSWORD = "rp";
    TypedLobby lobby1 = new TypedLobby("lobby1", LobbyType.Default);
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        MenuManager.Instance.OpenMenu("mainmenu");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Update()
    {

        if (!startGameButton.activeInHierarchy) return;
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            startGameButton.GetComponent<Button>().interactable = false;
            startGameButton.GetComponent<TMP_Text>().text = "Waiting for second player";
        }
        else
        {
            startGameButton.GetComponent<Button>().interactable = true;
            startGameButton.GetComponent<TMP_Text>().text = "START GAME";

        }

    }

    public void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();

        string[] lobbyProperties = { ROOM_PASSWORD };

        roomOptions.CustomRoomPropertiesForLobby = lobbyProperties;
        roomOptions.CustomRoomProperties = new Hashtable { { ROOM_PASSWORD, createRoomPassword.text } };
        roomOptions.MaxPlayers = 2;

        if (string.IsNullOrEmpty(createRoomName.text))
        {
            MenuManager.Instance.OpenPopUpMenu("popupdialogempty");
            return;
        }
        else
        {
            PhotonNetwork.CreateRoom(createRoomName.text, roomOptions, null);
            MenuManager.Instance.OpenMenu("loadingmenu");
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        print("createRoomError");
        errorText.text = "Room creation failed: " + message;
        MenuManager.Instance.OpenMenu("popupdialogcreation");
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        
        if (message == "Game full")
        {
            MenuManager.Instance.OpenMenu("popupdialoggameisfull");
        }
        else
        {
            MenuManager.Instance.OpenMenu("popupdialogjoinerror");
        }
    }

    public void JoinRoom()
    {
        if (joinRoomPassword.text == _password)
        {
            PhotonNetwork.JoinRoom(_roomName);
            MenuManager.Instance.OpenMenu("loadingmenu");
        }
        else
        {
            joinRoomPassword.placeholder.name = "Wrong password";
            joinRoomPassword.text = "";

        }
    }

    public override void OnJoinedRoom()
    {

        MenuManager.Instance.OpenMenu("roommenu");
        roomNameText.text = PhotonNetwork.CurrentRoom.Name;

        foreach (Transform child in playerListContent)
            Destroy(child.gameObject);

        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
            Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(players[i]);

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void PasswordBeforeJoinRoom(string password, string roomName)
    {

        MenuManager.Instance.OpenMenu("roompassword");
        _password = password;
        _roomName = roomName;
    }

    public void StartGame()
    {
        MenuManager.Instance.OpenMenu("loadingmenu");
        PhotonNetwork.CurrentRoom.IsOpen = true;
        PhotonNetwork.CurrentRoom.IsVisible = true;
        PhotonNetwork.LoadLevel(2);
    }

    public void LeaveRoom()
    {
        print("leave");
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(playerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnLeftRoom()
    {
        print("leaved");

        MenuManager.Instance.OpenMenu("mainmenu");
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }


    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        print("asd11");
        foreach (RoomInfo room in roomList)
        {
            print(room.Name);
        }
        if (Time.time >= nextUpdateTime)
        {
            UpdateRoomList(roomList);
            nextUpdateTime = Time.time + timeBetweenUptades;
        }
    }

    private void UpdateRoomList(List<RoomInfo> roomList)
    {

        foreach (RoomInfo room in roomList)
        {
            print(roomList.Count);
            if (room.RemovedFromList)
            {
                for (int i = 0; i < roomItemManagers.Count; i++)
                {
                    if (roomItemManagers[i].roomName.text == room.Name)
                    {
                        Destroy(roomItemManagers[i].gameObject);
                        roomItemManagers.RemoveAt(i);
                    }
                }
            }
            if (!room.RemovedFromList)
            {
                bool skip = false;
                for (int i = 0; i < roomItemManagers.Count; i++)
                {
                    if (roomItemManagers[i].roomName.text == room.Name)
                    {
                        skip = true;
                    }
                }
                if (!skip)
                {
                    RoomItemManager newRoom = Instantiate(roomItemPrefab, roomListContent);
                    newRoom.SetRoomName(room.Name);
                    newRoom.SetRoomPass(room.CustomProperties[ROOM_PASSWORD].ToString());
                    roomItemManagers.Add(newRoom);
                    if (room.PlayerCount == 0)
                    {
                        Destroy(newRoom.gameObject);
                        roomItemManagers.Remove(newRoom);
                    }
                }
            }
        }
    }
    //public void RoomMenuGoBack()
    //{

    //    MenuManager.Instance.OpenMenu("mainmenu");
    //    PhotonNetwork.LeaveRoom();
    //}
}