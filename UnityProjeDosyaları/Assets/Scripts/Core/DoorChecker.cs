using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PhotonView))]
public class DoorChecker : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] float rayLenght = 3f;
    float doorStayOpenTime = 5f;
    float TimeForTrigger = 0f;
    PhotonView photonView;

    [SerializeField] AudioSource checkerSoundCorrect;
    [SerializeField] AudioSource checkerSoundIncorrect;


    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        doorStayOpenTime = door.GetComponent<Door>().closeDoorTime;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(gameObject.transform.position, gameObject.transform.forward * rayLenght);
    }
    private void Update()
    {
        CheckForBody();
    }
    private void CheckForBody()
    {
        Ray ray = new Ray(gameObject.transform.position, gameObject.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLenght))
        {
            if (hit.transform.CompareTag("Security"))
            {
                if (Time.time > TimeForTrigger)
                {
                    photonView.RPC("TriggerTheDoor", RpcTarget.All);
                }
               
            }
            else
            {
                if (!checkerSoundIncorrect.isPlaying)
                {
                    checkerSoundIncorrect.Play();
                }
            }
        }
    }
    [PunRPC]
    private void TriggerTheDoor()
    {
        door.GetComponent<PhotonView>().RPC("Open",RpcTarget.MasterClient, false, 100, 1);  
        TimeForTrigger = doorStayOpenTime + Time.time;
        if (!checkerSoundCorrect.isPlaying)
        {
        checkerSoundCorrect.Play();
        }
    }

   
}
