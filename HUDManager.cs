using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//用于管理HUD UI，显示玩家的弹药、武器和投掷物信息
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

    public Sprite emptySlot;//没装备武器时用的
    public Sprite greySlot;//没投掷物时用的
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
            /*Mike教的是 return Instantiate(Resources.Load<GameObject>("Pistol1_Weapon")).GetComponent<SpriteRenderer>().sprite;
              但是这样一捡起武器每帧都会生成它们的sprite，于是我把Instantiate删了*/
            case Weapon.WeaponModel.MCX:
                return Resources.Load<GameObject>("MCX_Weapon").GetComponent<SpriteRenderer>().sprite;
            default:
                return null; // 如果没有匹配的武器模型，返回null
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
                return weaponSlot;//返回未激活的武器槽
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
