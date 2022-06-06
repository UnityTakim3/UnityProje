using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;
public class GunInputController : MonoBehaviour
{
    GunInputs gunInput;
    PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
        gunInput = new GunInputs();
    }
    private void OnEnable()
    {
        gunInput.Enable();
        gunInput.Shoot.RightClick.performed += RightClick;
        gunInput.Shoot.RightClick.canceled += RightClickOff;
        gunInput.ChangeSettings.MouseScroll.performed += MouseScroll;
        gunInput.ChangeSettings.KeyboardE.performed += KeyboardE;
        gunInput.ChangeSettings.KeyboardQ.performed += KeyboardQ;
        gunInput.ChangeGun.ChangeGun.performed += ChangeGun;

        gunInput.Shoot.LeftClick.performed += LeftClick;
    }


    private void OnDisable()
    {
        gunInput.Disable();

        gunInput.Shoot.RightClick.performed -= RightClick;
        gunInput.Shoot.RightClick.canceled -= RightClickOff;
        gunInput.ChangeSettings.MouseScroll.performed -= MouseScroll;
        gunInput.ChangeSettings.KeyboardE.performed -= KeyboardE;
        gunInput.ChangeSettings.KeyboardQ.performed -= KeyboardQ;
        gunInput.ChangeGun.ChangeGun.performed -= ChangeGun;

        gunInput.Shoot.LeftClick.performed -= LeftClick;
    }
   
    private void RightClick(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        FindEnabledGun().RightClick();
    }

    private void RightClickOff(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        FindEnabledGun().RightClickOff();
    } 
    private void LeftClick(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        FindEnabledGun().LeftClick();
    }
    private void MouseScroll(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        
            if (context.ReadValue<float>() > 0)
            {
                FindEnabledGun().MiddleMouseUp();

            }
            if (context.ReadValue<float>() < 0)
            {
                FindEnabledGun().MiddleMouseDown();

            }
        
        
    }
    private void KeyboardE(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        FindEnabledGun().KeyboardE();
    }
    private void KeyboardQ(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        FindEnabledGun().KeyboardQ();
    }

    private void ChangeGun(InputAction.CallbackContext context)
    {
        if (!photonView.IsMine) return;
        GetComponent<GunController>().ChangeGun();
    }

    private Gun FindEnabledGun()
    {
        Gun[] gunScripts = GetComponents<Gun>();
        foreach (var item in gunScripts)
        {
            if (item.enabled == false) continue;
            return item;
        }
        return null;
    }
}
