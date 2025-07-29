using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// �����࣬�����������������������׼�ȹ���
public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage; // �����˺�

    [Header("Weapon Properties")]
    //��������
    public GameObject bulletPrefab;
    public float bulletVelocity=30f;
    public Transform bulletSpawn;
    public float bulletPrefabLifeTime=3f;

    public GameObject muzzleEffect;
    internal Animator animator;

    [Header("Shooting Properties")]
    //�������
    public bool isShooting, readyToShoot;
    bool allowReset=true;
    public float shootingDelay=2f;

    //����ģʽ
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    [Header("Reloading Properties")]
    //��������
    public bool isReloading;
    public float reloadTime;
    public int magazineSize,bulletsLeft;

    public Vector3 spawnPositions;
    public Vector3 spawnRotation;

    public bool isADS;

    public enum WeaponModel
    {
        Pistol1,
        MCX
    }

    public WeaponModel thisWeaponModel;

    [Header("Spread Properties")]
    //ɢ��
    private float spreadIntensity;
    public float adsspreadIntensity;
    public float hipfireSpreadIntensity;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void EnterADS()
    {
        animator.SetTrigger("enterADS");
        isADS = true;
        HUDManager.Instance.middleDot.SetActive(false);
        spreadIntensity = adsspreadIntensity;
    }

    private void ExitADS()
    {
        animator.SetTrigger("exitADS");
        isADS = false;
        HUDManager.Instance.middleDot.SetActive(true);
        spreadIntensity = hipfireSpreadIntensity;
    }

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        animator = GetComponent<Animator>();
        bulletsLeft = magazineSize;
        spreadIntensity = hipfireSpreadIntensity;
    }

    void Update()
    {
        if(isActiveWeapon)
        {
            foreach(Transform child in transform)//��������������Ĳ�ΪWeaponRender��������ҳ�������ʱ��������һ���������Ⱦ
            {
                child.gameObject.layer = LayerMask.NameToLayer("WeaponRender");
            }

            if (Input.GetMouseButtonDown(1))
            {
                EnterADS();
            }
            if (Input.GetMouseButtonUp(1))
            {
               ExitADS();
            }
        }

        if (isActiveWeapon)
        {
            GetComponent<Outline>().enabled = false;
            if (bulletsLeft == 0 && isShooting)
            {
                //���û���ӵ��ˣ����ſյ���Ч
                SoundManager.Instance.emptyMagazineSound.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                //��ס���
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Burst || currentShootingMode == ShootingMode.Single)
            {
                //������
                isShooting = Input.GetKeyDown(KeyCode.Mouse0);
            }

            if (isShooting && readyToShoot && bulletsLeft > 0)
            {
                burstBulletsLeft = bulletsPerBurst;
                FireWeapon();
            }

            if (Input.GetKeyDown(KeyCode.R) && !isReloading && bulletsLeft < magazineSize &&WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel)>0)
            {
                Reload();
            }

            //�Զ�װ��
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                //Reload();
            }

        }

        else//��������������Ĳ�ΪĬ�ϲ㣬������Ҳ���������ʱ�����������������Ⱦ
        {
            foreach (Transform child in transform)
            {
                child.gameObject.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    private void FireWeapon()
    {
        bulletsLeft--;
        muzzleEffect.GetComponent<ParticleSystem>().Play();

        if(isADS)
        {
            //������ADS״̬������ADS����
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            //���򲥷����䶯��
            animator.SetTrigger("RECOIL");
        }

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        //��һ�ο���δ���ʱ������еڶ��ο���
        readyToShoot = false;
        
        Vector3 shootingDirection=CalculateDirectionAndSpread().normalized;

        //�����ӵ�����ʹ��raycast
        GameObject bullet=Instantiate(bulletPrefab,bulletSpawn.position,Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        bullet.transform.forward = shootingDirection;

        //Ϊ�ӵ������
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity,ForceMode.Impulse);

        //ʹ��Э��ɾ���ӵ�
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));

        //����Ƿ��Ѿ���ɿ�����Ϊ
        if(allowReset)
        {
            Invoke("ResetShot",shootingDelay);
            allowReset = false;
        }

        if(currentShootingMode==ShootingMode.Burst&&burstBulletsLeft>1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void Reload()
    {
        SoundManager.Instance.PlayReloadSound(thisWeaponModel);
        animator.SetTrigger("RELOAD");
        isReloading = true;
        Invoke("ReloadComplete", reloadTime);
    }

    private void ReloadComplete()
    {
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel)>magazineSize)//ʣ�µĵ�ҩ������������
        {
            bulletsLeft= magazineSize;
            WeaponManager.Instance.DecreaseTotalAmmo(thisWeaponModel, magazineSize);
        }
        else
        {
            bulletsLeft = WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel);
            WeaponManager.Instance.DecreaseTotalAmmo(thisWeaponModel, bulletsLeft);
        }
            isReloading = false;
    }

    //?Ϊ�˽�������һ�ο�����Ϊ
    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray,out hit))
        {
            //���߻������壬ʹ�û��е���ΪĿ���
            targetPoint = hit.point;
        }
        else
        {
            //����δ�������壬ʹ�����ߵ��յ�
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint-bulletSpawn.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0, y, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)//����ֵΪ������
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
