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
        //生成子弹而非使用raycast
        GameObject bullet=Instantiate(bulletPrefab,bulletSpawn.position,Quaternion.identity);
        //为子弹添加力
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawn.forward.normalized*bulletVelocity,ForceMode.Impulse);
        //使用协程删除子弹
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)//返回值为迭代器
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
