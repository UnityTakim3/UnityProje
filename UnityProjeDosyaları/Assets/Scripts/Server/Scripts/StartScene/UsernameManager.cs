using TMPro;
using UnityEngine;
using Photon.Pun;

public class UsernameManager : MonoBehaviour
{
    [SerializeField] TMP_InputField usernameInputField;

    private void Start()
    {
        OnUsernameInputValueChanged();
    }

    public void OnUsernameInputValueChanged()
    {
        PhotonNetwork.NickName = usernameInputField.text;
    }
}
