using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomItem : MonoBehaviour
{
    public Text roomName;
    public string roomPassword;
    CreateRoomOrJoin lobbyManager;

    private void Start()
    {
        lobbyManager = FindObjectOfType<CreateRoomOrJoin>();   
    }
    public void JoinCreatedRoom()
    {
        lobbyManager.PasswordBeforeJoinRoom(roomPassword,roomName.text);
    }
    public void SetRoomName(string _roomName)
    {
        roomName.text = _roomName;
    }
    public void SetRoomPass(string _roomPass)
    {
        roomPassword = _roomPass;
    }
}
