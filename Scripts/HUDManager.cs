using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//���ڹ���HUD UI����ʾ��ҵĵ�ҩ��������Ͷ������Ϣ
public class HUDManager : MonoBehaviour
{
    public static HUDManager Instance { get; private set; }

    [Header("Ammo")]
    public TextMeshProUGUI magazineAmmoUI;
    public TextMeshProUGUI totalAmmoUI;
    public Image ammoTypeUI;

    [Header("Weapon")]
    public Image activeWeaponUI;
    public Image unActiveWeaponUI;

    [Header("Throwables")]
    public Image lethalUI;
    public TextMeshProUGUI lethalAmmountUI;

    public Image tacticalUI;
    public TextMeshProUGUI tacticalAmmountUI;

    public Sprite emptySlot;//ûװ������ʱ�õ�
    public Sprite greySlot;//ûͶ����ʱ�õ�
    public GameObject middleDot; 

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }

    private void Update()
    {
        Weapon activeWeapon=WeaponManager.Instance.activeWeaponSlot.GetComponentInChildren<Weapon>();
        Weapon unActiveWeapon=GetUnActiveWeaponSlot().GetComponentInChildren<Weapon>();

        if(activeWeapon)
        {
            magazineAmmoUI.text =$"{activeWeapon.bulletsLeft/activeWeapon.bulletsPerBurst}";
            totalAmmoUI.text = $"{WeaponManager.Instance.CheckAmmoLeftFor(activeWeapon.thisWeaponModel)}";

            Weapon.WeaponModel model = activeWeapon.thisWeaponModel;
            ammoTypeUI.sprite = GetAmmoSprite(model);
            activeWeaponUI.sprite = GetWeaponSprite(model);

            if(unActiveWeapon)
            {
                unActiveWeaponUI.sprite= GetWeaponSprite(unActiveWeapon.thisWeaponModel);
            }
        }

        else
        {
            magazineAmmoUI.text = "";
            totalAmmoUI.text = "";

            ammoTypeUI.sprite = emptySlot;
            activeWeaponUI.sprite = emptySlot;
            unActiveWeaponUI.sprite = emptySlot;
        }

        if (WeaponManager.Instance.lethalsCount<=0)
            lethalUI.sprite = greySlot;

        if (WeaponManager.Instance.tacticalsCount <= 0)
            tacticalUI.sprite = greySlot;
    }

    private Sprite GetWeaponSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol1:
                return Resources.Load<GameObject>("Pistol1_Weapon").GetComponent<SpriteRenderer>().sprite;
            /*Mike�̵��� return Instantiate(Resources.Load<GameObject>("Pistol1_Weapon")).GetComponent<SpriteRenderer>().sprite;
              ��������һ��������ÿ֡�����������ǵ�sprite�������Ұ�Instantiateɾ��*/
            case Weapon.WeaponModel.MCX:
                return Resources.Load<GameObject>("MCX_Weapon").GetComponent<SpriteRenderer>().sprite;
            default:
                return null; // ���û��ƥ�������ģ�ͣ�����null
        }
    }

    private Sprite GetAmmoSprite(Weapon.WeaponModel model)
    {
        switch (model)
        {
            case Weapon.WeaponModel.Pistol1:
                return Resources.Load<GameObject>("Pistol_Ammo").GetComponent<SpriteRenderer>().sprite;
            case Weapon.WeaponModel.MCX:
                return Resources.Load<GameObject>("Rifle_Ammo").GetComponent<SpriteRenderer>().sprite;
            default:
                return null;
        }
    }

    private GameObject GetUnActiveWeaponSlot()
    {
        foreach (GameObject weaponSlot in WeaponManager.Instance.weaponSlots)
        {
            if (weaponSlot != WeaponManager.Instance.activeWeaponSlot)
            {
                return weaponSlot;//����δ�����������
            }
        }
        return null;
    }

    public void UpdateThrowablesUI()
    {
        lethalAmmountUI.text = $"{WeaponManager.Instance.lethalsCount}";
        tacticalAmmountUI.text = $"{WeaponManager.Instance.tacticalsCount}";

        switch (WeaponManager.Instance.equippedLethalType)
        {
            case Throwable.ThrowableType.Grenade:
                lethalUI.sprite = Resources.Load<GameObject>("Grenade").GetComponent<SpriteRenderer>().sprite;
                break;
        }

        switch (WeaponManager.Instance.equippedTacticalType)
        {
            case Throwable.ThrowableType.Smokebomb:
                lethalUI.sprite = Resources.Load<GameObject>("Smokebomb").GetComponent<SpriteRenderer>().sprite;
                break;
        }

    }
}
