using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowCamera : MonoBehaviour
{
    GameObject target = null;
    CinemachineVirtualCamera virtualCamera;
    private void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    void FixedUpdate()
    {
        if (target!= null)
        {
            Destroy(this);
        }
        try
        {
            target = GameObject.FindWithTag("CinemachineTarget");
            virtualCamera.Follow = target.transform;
        }
        catch (Exception)
        {

           
        }
       
    }
}
