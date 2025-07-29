using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float timeForDestruction;
    private void Start()
    {
        StartCoroutine(SelfDestruct(timeForDestruction));
    }

    private IEnumerator SelfDestruct(float timeForDestruction)
    {
        yield return new WaitForSeconds(timeForDestruction);
        Destroy(gameObject);
    }
}
