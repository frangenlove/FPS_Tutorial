using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��������ơ��ƿ����Ч������
public class BeerBottle : MonoBehaviour
{
    public List<Rigidbody>allParts= new List<Rigidbody>();

    public void Shatter()
    {
        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = false;//��������Ч��
        }
    }
}
