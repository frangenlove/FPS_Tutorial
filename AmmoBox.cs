using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//用于存储弹药箱的属性
public class AmmoBox : MonoBehaviour
{
    public int ammoAmount;
    public AmmoType ammoType;

    public enum AmmoType
    {
        PistolAmmo,
        RifleAmmo
    }
}
