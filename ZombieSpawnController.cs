using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ZombieSpawnController : MonoBehaviour
{
    public GameObject zombiePrefab;

    [Header("UI")]
    public TextMeshProUGUI waveOverUI;
    public TextMeshProUGUI coolDownCounterUI;
    public TextMeshProUGUI currentWaveUI;

    public int initialZombiesPerWave = 5;
    public int currentZombiesPerWave;

    public int currentWave = 0;

    public float spawnDelay = 1f;
    public float waveCooldown = 5f;

    public bool inCooldown;
    public float cooldownCounter=0;

    public List<Enemy> currentZombieAlive;

    private void Start()
    {
        currentZombiesPerWave= initialZombiesPerWave;
        GlobalReferences.Instance.waveNumber = currentWave;
        StartNextWave(); 
    }

    private void StartNextWave()
    {
        currentZombieAlive.Clear();
        currentWave++;
        GlobalReferences.Instance.waveNumber = currentWave;
        currentWaveUI.text = "Wave: " + currentWave.ToString();
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        for (int i=0;i<currentZombiesPerWave;i++)
        {
            Vector3 spawnOffset = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f));
            Vector3 spawnPosition = transform.position + spawnOffset;


            var zombie = Instantiate(zombiePrefab, spawnPosition, Quaternion.identity);//生成僵尸

            Enemy enemyScript = zombie.GetComponent<Enemy>();//取得enemy脚本

            currentZombieAlive.Add(enemyScript);

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    private void Update()
    {
        //获取已死僵尸
        List<Enemy>zombiesToRemove = new List<Enemy>();
        foreach (Enemy zombie in currentZombieAlive)
        {
            if (zombie.isDead)
            {
                zombiesToRemove.Add(zombie);
            }
        }

        //移除已死僵尸
        foreach (Enemy zombie in zombiesToRemove)
        {
            currentZombieAlive.Remove(zombie);
        }
        zombiesToRemove.Clear();

        if(currentZombieAlive.Count==0&&inCooldown==false)
        {
            StartCoroutine(WaveCooldown());
        }

        if(inCooldown)
        {
            cooldownCounter -= Time.deltaTime;
        }
        else
        {
            cooldownCounter = waveCooldown;//重置冷却计时器
        }

        coolDownCounterUI.text = cooldownCounter.ToString("F0");
    }

    private IEnumerator WaveCooldown()
    {
        inCooldown= true;
        waveOverUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(waveCooldown);

        inCooldown = false;
        waveOverUI.gameObject.SetActive(false);
        currentZombiesPerWave += 2;
        StartNextWave();
    }
}
