using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 用于处理子弹碰撞的类
public class Bullet : MonoBehaviour
{
    public int bulletDamage; // 子弹伤害

    private void OnCollisionEnter(Collision objectWeHit)
    {
        if(objectWeHit.gameObject.CompareTag("Target"))
        {
            print("hit"+ objectWeHit.gameObject.name+"!");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }
        if(objectWeHit.gameObject.CompareTag("Wall"))
        {
            print("hit a wall");
            CreateBulletImpactEffect(objectWeHit);
            Destroy(gameObject);
        }   
        if(objectWeHit.gameObject.CompareTag("Beer"))
        {
            print("hit a Beer");
            objectWeHit.gameObject.GetComponent<BeerBottle>().Shatter();
            
        }   
        if(objectWeHit.gameObject.CompareTag("Enemy"))
        {
            if(objectWeHit.gameObject.GetComponent<Enemy>().isDead==false)
            {
                objectWeHit.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);//这样敌人死后就不会再受到伤害且不会触发动画了
            }
            CreateBloodSprayEffcet(objectWeHit);
            Destroy(gameObject);
        }

    }

    private void CreateBloodSprayEffcet(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject bloodSprayPrefab = Instantiate(
            GlobalReferences.Instance.bloodSprayEffect,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );
        bloodSprayPrefab.transform.SetParent(objectWeHit.gameObject.transform);
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject hole = Instantiate(
            GlobalReferences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );
        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
