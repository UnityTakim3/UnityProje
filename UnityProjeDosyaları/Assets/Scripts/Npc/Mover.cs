using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public class Mover : MonoBehaviour, IAction
{
    NavMeshAgent _agent;
    Animator _animator;
    [SerializeField] float maxSpeed = 3.5f;
    private void Start()
    {
        
        _animator = GetComponentInChildren<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        maxSpeed = _agent.speed;
    }
    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
        SetAnimationSpeed();
        }
    }

    public void MoveTo(Vector3 position,float speed)
    {
        GetComponent<ActionScheduler>().StartAction(this);
        Move(position,speed);
    }
    public void Move(Vector3 position,float speed)
    {
        _agent.isStopped = false;
        _agent.SetDestination(position);
       _agent.speed=maxSpeed* Mathf.Clamp01(speed);
    }
    public void Cancel()
    {
        _agent.isStopped = true;
    }

    void SetAnimationSpeed()
    {
        Vector3 velocity = _agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);
        float speed = localVelocity.z;
        _animator.SetFloat("forwardSpeed", speed);
    }


}
