using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Door : MonoBehaviour
{
    Animator animator;
    public float closeDoorTime = 5f;
    [SerializeField] AudioSource doorOpenSound;
    [SerializeField] AudioSource doorCloseSound;

    [SerializeField] bool isOpened = false;
    bool isClosing = false;

    public int openPriority = -1;
    public int closePriority = -1;
    public int lastIdendity = -1;

    int startOpenPriority = 0;
    int startClosePriority = 0;

    PhotonView photonView;
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        startOpenPriority = openPriority;
        startClosePriority = closePriority;
        animator = GetComponentInChildren<Animator>();
    }
    [PunRPC]
    public void Open(bool StayOpen, int priority, int idendity)
    {
        if (isOpened) return;
        if (lastIdendity != idendity)
        {
            if (priority < openPriority) return;
        }
        closePriority = priority;
        if (closePriority == 0)
        {
            closePriority = startClosePriority;
        }
        lastIdendity = idendity;

        photonView.RPC("OpenDoor", RpcTarget.All);
        //animator.SetTrigger("open");
        
        if (isClosing || StayOpen) return;
        StartCoroutine(CloseTimer());
    }
    [PunRPC]
    public void Close(int priority, int idendity)
    {
        print("close");

        if (!isOpened) return;
        if (lastIdendity != idendity)
        {
            if (priority < closePriority) return;
        }
        openPriority = priority;
        if (openPriority == 0)
        {
            openPriority = startOpenPriority;
        }
        lastIdendity = idendity;
        print("close1");

        photonView.RPC("CloseDoor", RpcTarget.All);
        //animator.SetTrigger("close");
       
    }
    IEnumerator CloseTimer()
    {
        isClosing = true;
        yield return new WaitForSeconds(closeDoorTime);
        photonView.RPC("CloseDoor", RpcTarget.All);
        //animator.SetTrigger("close");
        doorCloseSound.Play();
    }
    public void IsOpenTrue()
    {
        isOpened = true;
    }
    public void IsOpenFalse()
    {
        isClosing = false;
        isOpened = false;
    }
    [PunRPC]
    private void OpenDoor()
    {
        //animator.SetTrigger("open"); 
        animator.SetBool("Open", true);
        animator.SetBool("Close", false);
        if (!doorOpenSound.isPlaying)
        {
            doorOpenSound.Play();
        }
    }
    [PunRPC]
    private void CloseDoor()
    {
        //animator.SetTrigger("close");
        animator.SetBool("Close", true);
        animator.SetBool("Open", false);
        if (!doorCloseSound.isPlaying)
        {
            doorCloseSound.Play();
        }

    }
}
