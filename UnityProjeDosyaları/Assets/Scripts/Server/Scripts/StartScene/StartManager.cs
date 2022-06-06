using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using TMPro;

public class StartManager : MonoBehaviourPunCallbacks
{
    [Header("TMP")]
    [SerializeField] TMP_Text connectButton;
    [SerializeField] TMP_InputField usernameInputField;

    public void ConnectLobby()
    {
        if (string.IsNullOrEmpty(usernameInputField.text))
        {
            PhotonNetwork.NickName = "Player " + Random.Range(0, 999999).ToString("000000");
            usernameInputField.text = PhotonNetwork.NickName;
        }
        connectButton.text = "Loading...";
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        SceneManager.LoadScene("MainUIScene");
    }

   
}
