using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// 武器类，负责武器的射击、换弹、瞄准等功能
public class Weapon : MonoBehaviour
{
    public bool isActiveWeapon;
    public int weaponDamage; // 武器伤害

    [Header("Weapon Properties")]
    //武器属性
    public GameObject bulletPrefab;
    public float bulletVelocity=30f;
    public Transform bulletSpawn;
    public float bulletPrefabLifeTime=3f;

    public GameObject muzzleEffect;
    internal Animator animator;

    [Header("Shooting Properties")]
    //射击属性
    public bool isShooting, readyToShoot;
    bool allowReset=true;
    public float shootingDelay=2f;

    //连发模式
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;

    [Header("Reloading Properties")]
    //换弹参数
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
    //散布
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
            foreach(Transform child in transform)//设置武器子物体的层为WeaponRender，仅在玩家持有武器时对其用另一相机进行渲染
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
                //如果没有子弹了，播放空弹音效
                SoundManager.Instance.emptyMagazineSound.Play();
            }

            if (currentShootingMode == ShootingMode.Auto)
            {
                //按住左键
                isShooting = Input.GetKey(KeyCode.Mouse0);
            }
            else if (currentShootingMode == ShootingMode.Burst || currentShootingMode == ShootingMode.Single)
            {
                //点击左键
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

            //自动装弹
            if (readyToShoot && !isShooting && !isReloading && bulletsLeft <= 0)
            {
                //Reload();
            }

        }

        else//设置武器子物体的层为默认层，仅在玩家不持有武器时对其用主相机进行渲染
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
            //若处于ADS状态，播放ADS动画
            animator.SetTrigger("RECOIL_ADS");
        }
        else
        {
            //否则播放腰射动画
            animator.SetTrigger("RECOIL");
        }

        SoundManager.Instance.PlayShootingSound(thisWeaponModel);

        //第一次开火未完成时不会进行第二次开火
        readyToShoot = false;
        
        Vector3 shootingDirection=CalculateDirectionAndSpread().normalized;

        //生成子弹而非使用raycast
        GameObject bullet=Instantiate(bulletPrefab,bulletSpawn.position,Quaternion.identity);

        Bullet bul = bullet.GetComponent<Bullet>();
        bul.bulletDamage = weaponDamage;

        bullet.transform.forward = shootingDirection;

        //为子弹添加力
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletVelocity,ForceMode.Impulse);

        //使用协程删除子弹
        StartCoroutine(DestroyBulletAfterTime(bullet,bulletPrefabLifeTime));

        //检查是否已经完成开火行为
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
        if(WeaponManager.Instance.CheckAmmoLeftFor(thisWeaponModel)>magazineSize)//剩下的弹药可以填满弹夹
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

    //?为了仅会重置一次开火行为
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
            //射线击中物体，使用击中点作为目标点
            targetPoint = hit.point;
        }
        else
        {
            //射线未击中物体，使用射线的终点
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint-bulletSpawn.position;

        float z = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        return direction + new Vector3(0, y, z);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)//返回值为迭代器
    {
        yield return new WaitForSeconds(delay);
        Destroy(bullet);
    }
}
