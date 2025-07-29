using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 用于制作啤酒瓶破碎效果的类
public class BeerBottle : MonoBehaviour
{
    public List<Rigidbody>allParts= new List<Rigidbody>();

    public void Shatter()
    {
        foreach (Rigidbody part in allParts)
        {
            part.isKinematic = false;//制作破碎效果
        }
    }
}
