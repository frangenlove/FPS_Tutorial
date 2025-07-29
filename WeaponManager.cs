using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Weapon;

//���ڹ�����������
public class WeaponManager : MonoBehaviour
{
    public static WeaponManager Instance { get; private set; }

    public List<GameObject> weaponSlots;
    public GameObject activeWeaponSlot;

    [Header("Ammo")]
    public int totalPistolAmmo=0;
    public int totalRifleAmmo=0;

    [Header("Throwable General")]
    public float throwForce = 10f;
    public GameObject throwableSpawn;
    public float forceMultiplier = 0f;
    public float forceMultiplierLimit=2f;

    [Header("Lethals")]
    public int maxLethalsCount = 5; //���Я������
    public int lethalsCount = 0;
    public Throwable.ThrowableType equippedLethalType;
    public GameObject grenadePrefab;

    [Header("Tacticals")]
    public int maxTacticalsCount = 5; //���Я������
    public int tacticalsCount = 0;
    public Throwable.ThrowableType equippedTacticalType;
    public GameObject smokebombPrefab;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Start()
    {
        activeWeaponSlot = weaponSlots[0];
        equippedLethalType = Throwable.ThrowableType.None;
        equippedTacticalType = Throwable.ThrowableType.None;
    }

    private void Update()
    {
        foreach(GameObject weaponSlot in weaponSlots)
        {
            if (weaponSlot==activeWeaponSlot)
                weaponSlot.SetActive(true);
            else
                weaponSlot.SetActive(false);

        }


        if(Input.GetKeyDown(KeyCode.Alpha1))
            SwitchActiveWeapon(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            SwitchActiveWeapon(1);

        if(Input.GetKey(KeyCode.G)|| Input.GetKey(KeyCode.T))
        {
            forceMultiplier += Time.deltaTime;//��סG����Ͷ������
            if(forceMultiplier>forceMultiplierLimit)
                forceMultiplier = forceMultiplierLimit; //�������Ͷ������
        }
            

        if (Input.GetKeyUp(KeyCode.G))
        {
            if (lethalsCount > 0)
            {
                throwLethal();
            }

            forceMultiplier = 0f; //�ɿ�G������Ͷ������
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            if (tacticalsCount > 0)
            {
                throwTactical();
            }

            forceMultiplier = 0f; //�ɿ�T������Ͷ������
        }
    }

    #region||�����������||
    public void PickupWeapon(GameObject pickedupWeapon)
    {
        //
        AddWeaponIntoActiveSlot(pickedupWeapon);
    }

    private void AddWeaponIntoActiveSlot(GameObject pickupedWeapon)
    {
        DropCurrentWeapon(pickupedWeapon);

        pickupedWeapon.transform.SetParent(activeWeaponSlot.transform,false);

        Weapon weapon = pickupedWeapon.GetComponent<Weapon>();

        pickupedWeapon.transform.localPosition = new Vector3(weapon.spawnPositions.x,weapon.spawnPositions.y,weapon.spawnPositions.z);
        pickupedWeapon.transform.localRotation = Quaternion.Euler(weapon.spawnRotation.x, weapon.spawnRotation.y, weapon.spawnRotation.z);
        
        weapon.isActiveWeapon = true;
        weapon.animator.enabled = true;
    }

    private void DropCurrentWeapon(GameObject pickupedWeapon)
    {
        if(activeWeaponSlot.transform.childCount>0)
        {
            var weaponToDrop= activeWeaponSlot.transform.GetChild(0).gameObject;
            weaponToDrop.GetComponent<Weapon>().isActiveWeapon = false;
            weaponToDrop.GetComponent<Weapon>().animator.enabled = false;

            weaponToDrop.transform.SetParent(pickupedWeapon.transform.parent);
            weaponToDrop.transform.localPosition = pickupedWeapon.transform.localPosition;
            weaponToDrop.transform.localRotation = pickupedWeapon.transform.localRotation;
        }
    }

    public void SwitchActiveWeapon(int slotNumber)
    {
        if(activeWeaponSlot.transform.childCount>0)
        {
            Weapon currentWeapon= activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = false;
        }

        activeWeaponSlot = weaponSlots[slotNumber];

        if (activeWeaponSlot.transform.childCount > 0)
        {
            Weapon currentWeapon = activeWeaponSlot.transform.GetChild(0).GetComponent<Weapon>();
            currentWeapon.isActiveWeapon = true;
        }
    }
    #endregion

    #region ||��ҩ���||
    internal void PickupAmmo(AmmoBox ammoBox)
    {
        switch (ammoBox.ammoType)
        {
            case AmmoBox.AmmoType.PistolAmmo:
                totalPistolAmmo += ammoBox.ammoAmount;
                break;
            case AmmoBox.AmmoType.RifleAmmo:
                totalRifleAmmo  += ammoBox.ammoAmount;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

    }

    internal void DecreaseTotalAmmo(Weapon.WeaponModel thisWeaponModel, int bulletsToDecrease)
    {
        switch (thisWeaponModel)
        {
            case Weapon.WeaponModel.Pistol1:
                totalPistolAmmo -= bulletsToDecrease;
                break;
            case Weapon.WeaponModel.MCX:
                totalRifleAmmo -= bulletsToDecrease;
                break;
        }
    }

    public int CheckAmmoLeftFor(WeaponModel thisWeaponModel)
    {
        switch (thisWeaponModel)
        {
            case WeaponModel.Pistol1:
                return totalPistolAmmo;
            case WeaponModel.MCX:
                return totalRifleAmmo;
            default:
                return 0;
        }
    }
    #endregion

    #region ||Ͷ�������||

    //����ʰȡͶ����
    internal void PickupThrowable(Throwable throwable)
    {
        switch(throwable.throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                PickupThrowableAsLethal(Throwable.ThrowableType.Grenade);
                break;
            case Throwable.ThrowableType.Smokebomb:
                PickupThrowableAsTactical(Throwable.ThrowableType.Smokebomb);
                break;
        }
    }

    //���ڸ�����ҵ�Ͷ����UI��������ϵ�Ͷ�������ԣ���ͬ
    private void PickupThrowableAsTactical(Throwable.ThrowableType tactical)
    {
        if (equippedTacticalType == tactical || equippedTacticalType == Throwable.ThrowableType.None)
        {
            equippedTacticalType = tactical;
            if (tacticalsCount < maxLethalsCount)//�������Я������Ϊ5
            {
                tacticalsCount++;
                Destroy(InteractionManager.Instance.hoverdThrowable.gameObject);
                HUDManager.Instance.UpdateThrowablesUI();
            }
            else
            {
                print("You already have maximum amount of tacticals.");
            }
        }
        else
        {

        }
    }

    private void PickupThrowableAsLethal(Throwable.ThrowableType lethal)
    {
        if(equippedLethalType==lethal || equippedLethalType==Throwable.ThrowableType.None)
        {
            equippedLethalType = lethal;
            if(lethalsCount<maxTacticalsCount)//�������Я������Ϊ5
            {
                lethalsCount++;
                Destroy(InteractionManager.Instance.hoverdThrowable.gameObject);
                HUDManager.Instance.UpdateThrowablesUI();
            }
            else
            {
                print("You already have maximum amount of lethals.");
            }
        }
        else
        {

        }
    }

    //����ִ��Ͷ�����׵���Ϊ
    private void throwLethal()
    {
        GameObject lethalPrefab = GetThrowablePrefab(equippedLethalType);
        GameObject throwable = Instantiate(lethalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);//forceMultiplier���ڵ���Ͷ������
        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        lethalsCount--;

        if(lethalsCount<=0)
            equippedLethalType = Throwable.ThrowableType.None;
        
        HUDManager.Instance.UpdateThrowablesUI();
    }


    //����ִ��Ͷ��ս����������Ϊ
    private void throwTactical()
    {
        GameObject tacticalPrefab = GetThrowablePrefab(equippedTacticalType);
        GameObject throwable = Instantiate(tacticalPrefab, throwableSpawn.transform.position, Camera.main.transform.rotation);
        Rigidbody rb = throwable.GetComponent<Rigidbody>();

        rb.AddForce(Camera.main.transform.forward * (throwForce * forceMultiplier), ForceMode.Impulse);//����There is no rigidbody attached to the new game object
        throwable.GetComponent<Throwable>().hasBeenThrown = true;

        tacticalsCount--;

        if (tacticalsCount <= 0)
            equippedTacticalType = Throwable.ThrowableType.None;

        HUDManager.Instance.UpdateThrowablesUI();
    }


    //���ڻ�ȡͶ����Ԥ����
    private GameObject GetThrowablePrefab(Throwable.ThrowableType throwableType)
    {
        switch (throwableType)
        {
            case Throwable.ThrowableType.Grenade:
                return grenadePrefab;
            case Throwable.ThrowableType.Smokebomb:
                return smokebombPrefab;
        }
        return new();
    }

    #endregion
}
