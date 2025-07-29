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
        //初始化
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent=animator.GetComponent<NavMeshAgent>();
        agent.speed =patrolSpeed;
        timer = 0f;

        //获取所有路径并移动至第一个路径
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

        //播放巡逻音效
        if (SoundManager.Instance.zombieChannel.isPlaying==false)
        {
            SoundManager.Instance.zombieChannel.clip=SoundManager.Instance.zombieWalking;
            SoundManager.Instance.zombieChannel.PlayDelayed(1f);
        }


        //如果到达路径点则移动至下一路径
        if(agent.remainingDistance<=agent.stoppingDistance)
        {
            agent.SetDestination(wayPointList[Random.Range(0, wayPointList.Count)].position);
        }

        //转至idleState
        if(timer>patrolTime)
        {
            animator.SetBool("isPatroling",false);
        }

        //转至chaseState
        float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
        if (distanceFromPlayer < detectionArea)
        {
            animator.SetBool("isChasing", true);
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //停止agent
        agent.SetDestination(agent.transform.position);
        SoundManager.Instance.zombieChannel.Stop();
    }
}
