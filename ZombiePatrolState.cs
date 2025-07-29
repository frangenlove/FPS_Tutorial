using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombiePatrolState : StateMachineBehaviour
{
    float timer;
    public float patrolTime=10f;

    Transform player;
    NavMeshAgent agent;

    public float detectionArea = 18f;
    public float patrolSpeed=2f;

    List<Transform> wayPointList= new List<Transform>(); 



    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //��ʼ��
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent=animator.GetComponent<NavMeshAgent>();
        agent.speed =patrolSpeed;
        timer = 0f;

        //��ȡ����·�����ƶ�����һ��·��
        GameObject wayPointCluster = GameObject.FindGameObjectWithTag("Waypoints");
        foreach (Transform t in wayPointCluster.transform)
        {
            wayPointList.Add(t);
        }
        Vector3 nextPosition = wayPointList[Random.Range(0,wayPointList.Count)].position;
        agent.SetDestination(nextPosition);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        //����Ѳ����Ч
        if (SoundManager.Instance.zombieChannel.isPlaying==false)
        {
            SoundManager.Instance.zombieChannel.clip=SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }


        //�������·�������ƶ�����һ·��
        if(agent.remainingDistance<=agent.stoppingDistance)
        {
            agent.SetDestination(wayPointList[Random.Range(0, wayPointList.Count)].position);
        }

        //ת��idleState
        if(timer>patrolTime)
        {
            animator.SetBool("isPatroling",false);
        }

        //ת��chaseState
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //ֹͣagent
        agent.SetDestination(agent.transform.position);
        SoundManager.Instance.zombieChannel.Stop();
    }
}
