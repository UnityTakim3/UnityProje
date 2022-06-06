using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.Events;
using Photon.Realtime;

public class NpcRobotController : MonoBehaviourPunCallbacks
{


    Fighter fighter;
    Mover mover;
    Health health;
    public GameObject[] players;
    int target = -22;

    public float sightDistance = 10f;
    [SerializeField] GameObject patrolPoints = null;

    [SerializeField] float waitingTimeInPatrolPoint = 2f, suspicionTime = 2f, moveSpeed = 0.2f;
    int patrolPoint = 0;
    float patrolPointTolerance = 0.5f;

    float timeWaitedInPatrolPoint = 0f;
    float timeSinceLastSawPlayer = 0f;

    PhotonView photonView;

    bool _isMasterClient = false;
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            _isMasterClient = true;

        }
        health = GetComponent<Health>();
        fighter = GetComponent<Fighter>();
        mover = GetComponent<Mover>();
        photonView = GetComponent<PhotonView>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, sightDistance);
    }
    void Update()
    {
        if (_isMasterClient)
        {
            UpdateServer();
            print("asdd111");
        }
        
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (_isMasterClient)
        {
            _isMasterClient = false;
        }
        else
        {
            _isMasterClient = true;
        }
    }

    private void UpdateServer()
    {

        if (!health.isAlive) return;

        timeWaitedInPatrolPoint += Time.deltaTime;
        timeSinceLastSawPlayer += Time.deltaTime;
        players = GameObject.FindGameObjectsWithTag("Player");
        LookForPlayers();
        if (target != -22)
        {
            //photonView.RPC("Attack", RpcTarget.All, target);
            fighter.Attack(target);
            timeSinceLastSawPlayer = 0f;
        }
        else if (timeSinceLastSawPlayer > suspicionTime)
        {
            StartPatrol();
        }
        else
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }
    }

    private void StartPatrol()
    {
        if (patrolPoints != null)
        {
            if (GetPatrolPointDistance())
            {
                timeWaitedInPatrolPoint = 0f;
                MoveToPatrolPoint();
                return;
            }
            if (timeWaitedInPatrolPoint > waitingTimeInPatrolPoint)
            {
                patrolPoint = NextPatrolPoint();
            }

        }

    }

    private bool GetPatrolPointDistance()
    {
        float pointDistance = Vector3.Distance(transform.position, patrolPoints.GetComponent<PatrolPoints>().getChild(patrolPoint));
        return pointDistance > patrolPointTolerance;
    }
    private void MoveToPatrolPoint()
    {
        //photonView.RPC("MoveTo", RpcTarget.All, patrolPoints.GetComponent<PatrolPoints>().getChild(patrolPoint), moveSpeed);
        mover.MoveTo(patrolPoints.GetComponent<PatrolPoints>().getChild(patrolPoint), moveSpeed);
    }
    private int NextPatrolPoint()
    {
        return patrolPoints.GetComponent<PatrolPoints>().getNextChild(patrolPoint);

    }

    void LookForPlayers()
    {
        GameObject firstPlayer = null;
        float firstDistance = -1;
        foreach (var player in players)
        {
            float distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
            if (firstDistance == -1)
            {
                firstDistance = distance;
                firstPlayer = player;
            }
            else
            {
                if (distance < firstDistance && distance < sightDistance)
                {
                    target = player.GetComponent<PhotonView>().ViewID;
                    return;
                }
                if (firstDistance < sightDistance)
                {
                    target = firstPlayer.GetComponent<PhotonView>().ViewID;
                }
                else
                {
                    target = -22;
                }

            }
        }



        //if (distance < sightDistance)
        //{
        //    target = player;
        //    return;
        //}
        //continue;
        //target = null;
    }
}
