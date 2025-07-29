using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 用于存储全局引用的类
public class GlobalReferences : MonoBehaviour
{
    public static GlobalReferences Instance { get; private set; }

    public GameObject grenadeExplosionEffect;
    public GameObject bulletImpactEffectPrefab;
    public GameObject smokebombEffect;
    public GameObject bloodSprayEffect;

    public int waveNumber;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;
    }
}
