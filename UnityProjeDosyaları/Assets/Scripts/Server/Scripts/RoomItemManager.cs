using UnityEngine;
using TMPro;

public class RoomItemManager : MonoBehaviour
{
    public TMP_Text roomName;
    public string roomPassword;
    Launcher launcher;

    private void Start()
    {
        launcher = FindObjectOfType<Launcher>();
    }
    public void JoinCreatedRoom()
    {
        launcher.PasswordBeforeJoinRoom(roomPassword, roomName.text);
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
