using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float bulletVelocity=30f;
    public Transform bulletSpawn;
    public float bulletPrefabLifeTime=3f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
            FireWeapon();
    }

    private void FireWeapon()
    {
        //�����ӵ�����ʹ��raycast
        GameObject bullet=Instantiate(bulletPrefab,bulletSpawn.position,Quaternion.identity);
        //Ϊ�ӵ������
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized*bulletVelocity,ForceMode.Impulse);
        //ʹ��Э��ɾ���ӵ�
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)//����ֵΪ������
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
