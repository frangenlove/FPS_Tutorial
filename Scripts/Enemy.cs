using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField] int HP = 100;
    private Animator animator;
    private NavMeshAgent navAgent;

    public bool isDead;

    private void Start()
    {
        animator = GetComponent<Animator>();
        navAgent = GetComponent<NavMeshAgent>();
    }

    public void TakeDamage(int damageAmount)
    {
        HP -= damageAmount;
        if (HP <= 0)
        {
            int RandomNumber = Random.Range(0, 2);//��0��1֮���������һ���������Ա��������������������ѡ��һ��
            if (RandomNumber == 0)
                animator.SetTrigger("DIE1");
            else
                animator.SetTrigger("DIE2");
            isDead = true;
            //ֻ��һ�ֵ�������ֱ����ôд
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieDeath);
            StartCoroutine(DestroyEnemy());
        }
        else
        {
            animator.SetTrigger("DAMAGE");
            SoundManager.Instance.zombieChannel2.PlayOneShot(SoundManager.Instance.zombieHurt);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,2.5f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, 100f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, 101f);
    }

    private IEnumerator DestroyEnemy()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

}
