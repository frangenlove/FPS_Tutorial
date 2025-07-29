using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���ڴ洢ȫ�����õ���
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
