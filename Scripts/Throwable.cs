using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//用于管理投掷物
public class Throwable : MonoBehaviour
{
    [SerializeField] float delay = 3f;
    [SerializeField] float damageRadius = 20f;
    [SerializeField] float explosionForce = 1200f;

    float countdown;

    public bool hasExploded = false;
    public bool hasBeenThrown = false;

    public enum ThrowableType
    {
        None,
        Grenade,
        Flashbomb,
        Smokebomb
    }

    public ThrowableType throwableType;

    private void Start()
    {
        countdown= delay;
    }

    private void Update()
    {
        if(hasBeenThrown)
        {
            countdown -= Time.deltaTime;
            if (countdown<=0f&&!hasExploded)
            {
                Exploded();
                hasExploded = true;
            }
        }
    }

    private void Exploded()
    {
        GetThrowableEffect();
        Destroy(gameObject);
    }

    private void GetThrowableEffect()
    {
        switch (throwableType)
        {
            case ThrowableType.Grenade:
                GrenadeEffect();// Implement grenade effect
                break;
            case ThrowableType.Flashbomb:
                FlashbombEffect();// Implement flashbomb effect
                break;
            case ThrowableType.Smokebomb:
                SmokebombEffcet();// Implement smoke bomb effect
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void SmokebombEffcet()
    {
        //产生视觉效果
        GameObject smokeEffect = GlobalReferences.Instance.smokebombEffect;
        Instantiate(smokeEffect, transform.position, transform.rotation);

        //产生声音效果
        SoundManager.Instance.throwableChannel.PlayOneShot(SoundManager.Instance.grenadeSound);

        //产生物理效果
        //记录范围内的所有collider
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach (Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.transform.GetComponent<Rigidbody>();
            if (rb != null)
            {
                
            }
        }

        //对敌人产生的效果
    }

    private void FlashbombEffect()
    {
        throw new NotImplementedException();
    }

    private void GrenadeEffect()
    {
        //产生视觉效果
        GameObject explosionEffect=GlobalReferences.Instance.grenadeExplosionEffect;
        Instantiate(explosionEffect,transform.position,transform.rotation);

        //产生声音效果
        SoundManager.Instance.throwableChannel.PlayOneShot(SoundManager.Instance.grenadeSound);

        //产生物理效果
        //记录范围内的所有collider
        Collider[] colliders = Physics.OverlapSphere(transform.position, damageRadius);
        foreach(Collider objectInRange in colliders)
        {
            Rigidbody rb = objectInRange.transform.GetComponent<Rigidbody>();
            if(rb!=null)
            {
                rb.AddExplosionForce(explosionForce,transform.position,damageRadius);
            }

            //对敌人产生的效果
            if (objectInRange.transform.GetComponent<Enemy>())
            {
                objectInRange.transform.GetComponent<Enemy>().TakeDamage(100);
            }
        }

    }
}