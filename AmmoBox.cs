using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���ڴ洢��ҩ�������
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
