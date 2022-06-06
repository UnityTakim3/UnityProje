using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorHelper : MonoBehaviour
{
    [SerializeField] Fighter fighter;
    [SerializeField] GunController gunController;

    Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void Hit()
    {
        fighter.Hit();
    }
    public void GunChanged()
    {
        gunController.GunChanged();
    }
    public void GunChangedFalse()
    {
        _animator.SetBool("ChangeGun", false);
    }
}
