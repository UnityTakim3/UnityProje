using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Switch : MonoBehaviour
{
    Animator switchAnim;
    public bool buttonActive = false;
    [SerializeField] int idendityNumber=1;
    
    [SerializeField] GameObject[] doorsWillOpen;
    [SerializeField] int[] doorsWillOpenPriorities;
    [SerializeField] GameObject[] doorsWillClose;
    [SerializeField] int[] doorsWillClosePriorities;
    [Header("Do you want to close the doors you have opened,or open you have closed when putton is unpressed?")]

    [SerializeField] bool openReverse = true;
    [SerializeField] bool closeReverse = true;
    
    void Start()
    {

        switchAnim = GetComponent<Animator>();

    }

    private void OnCollisionEnter(Collision collision)
    {
        SeizableObject seizable = collision.gameObject.GetComponent<SeizableObject>();

        if (seizable == null) return;
        buttonActive = true;

        if (buttonActive == true)
        {
            switchAnim.SetBool("isButtonPress", true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {

        SeizableObject seizable = collision.gameObject.GetComponent<SeizableObject>();
        if (seizable == null) return;
        switchAnim.SetBool("isButtonPress", false);
        buttonActive = false;
    }


    public void ButtonPressed() //butonun yere inme animasyonunun sonunda çalýþýyor
    {
        for (int i = 0; i < doorsWillOpen.Length; i++)
        {
            doorsWillOpen[i].GetComponent<PhotonView>().RPC("Open", RpcTarget.MasterClient, true, doorsWillOpenPriorities[i], idendityNumber);
        }
        
        for (int i = 0; i < doorsWillClose.Length; i++)
        {
            doorsWillClose[i].GetComponent<PhotonView>().RPC("Close", RpcTarget.MasterClient,doorsWillClosePriorities[i], idendityNumber);
        }
        
    }
    public void ButtonUnPressed()
    {
        if (openReverse)
        {
            foreach (GameObject item in doorsWillOpen)
            {
                item.GetComponent<PhotonView>().RPC("Close", RpcTarget.MasterClient, 0, idendityNumber); //Düþük ihtimal de olsa senkronizasyon hatasý olur da iki client de ayný kodu çalýþtýramaz diye kodu server'a yollayýp garanti olmasý için.Performans olarak zararlý
                //item.GetComponent<Door>().Close(0, idendityNumber);

            }
        }
        if (closeReverse)
        {
            foreach (GameObject item in doorsWillClose)
            {
                item.GetComponent<PhotonView>().RPC("Open", RpcTarget.MasterClient, true,0, idendityNumber);
               // item.GetComponent<Door>().Open(true, 0, idendityNumber);

            }
        }
       
    }
    [PunRPC]
    private void Deneme()
    {
        print("1");
    }
}
