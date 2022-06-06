using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Fighter : MonoBehaviour, IAction
{
    GameObject attackTarget;
    [SerializeField] float attackRange;
    Mover mover;
    PhotonView photonView;
    Animator animator;
    private void Start()
    {
        animator = transform.GetChild(0).GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        mover = GetComponent<Mover>();

    }
    void Update()
    {

        if (attackTarget == null) return;
        if (IsTargetInRange())
        {
            AttackBehavior();
        }
        else
        {
            //photonView.RPC("Chase", RpcTarget.All);
            Chase();
        }

    }
    private void Chase()
    {
        mover.Move(attackTarget.transform.position, 1);
        animator.ResetTrigger("hit");
        animator.SetTrigger("cancelHit");
    }

    private void AttackBehavior()
    {
        mover.Cancel();
        animator.ResetTrigger("cancelHit");
        animator.SetTrigger("hit");
    }
    public void Hit()
    {
        if (attackTarget == null) return;
        attackTarget.GetComponent<PhotonView>().RPC("GetDamage", RpcTarget.All);
    }
    public void Attack(int target)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        attackTarget = PhotonNetwork.GetPhotonView(target).gameObject;
    }
    bool IsTargetInRange()
    {
        float distance = Vector3.Distance(transform.position, attackTarget.transform.position);
        return distance < attackRange;
    }
    public void Cancel()
    {
        attackTarget = null;
        mover.Cancel();
    }
}
