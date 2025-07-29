using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

//用于管理声音的类
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioSource ShootingChannel;

    public AudioClip MCXShot;
    public AudioClip PistolShot;

    public AudioSource reloadingSoundPistol1;
    public AudioSource reloadingSoundMCX;

    public AudioSource emptyMagazineSound;

    public AudioSource throwableChannel;
    public AudioClip grenadeSound;

    public AudioSource zombieChannel;
    public AudioSource zombieChannel2;
    public AudioClip zombieAttack;
    public AudioClip zombieChase;
    public AudioClip zombieDeath;
    public AudioClip zombieWalking;
    public AudioClip zombieHurt;

    public AudioSource playerChannel;
    public AudioClip playerHurt;
    public AudioClip playerDead;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    public void PlayShootingSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1:                    
                ShootingChannel.PlayOneShot(PistolShot);
                break;
            case WeaponModel.MCX:
                ShootingChannel.PlayOneShot(MCXShot);
                break;
        }
    }

    public void PlayReloadSound(WeaponModel weapon)
    {
        switch (weapon)
        {
            case WeaponModel.Pistol1:
                reloadingSoundPistol1.Play();
                break;
            case WeaponModel.MCX:
                reloadingSoundMCX.Play();
                break;
        }
    }
}
