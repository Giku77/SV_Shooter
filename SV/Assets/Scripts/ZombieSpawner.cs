using UnityEngine;
using System.Collections.Generic;

public class ZombieSpawner : MonoBehaviour
{
    private Zombie zombiePrefab;

    //public ItemManager itemManager;

    public ZombieData[] zombieDatas;
    //public Transform[] spawnPoints;

    private List<Zombie> activeZombies = new List<Zombie>();

    //public UiManager uiManager;

    private int waveNumber;

    public int GetWaveNumber()
    {
        return waveNumber;
    }

    private void SpawnWave()
    {
        waveNumber++;

        int count = Mathf.RoundToInt(waveNumber * 1.5f);
        for (int i = 0; i < count; i++)
        {
            CreateZombie();
        }
        //uiManager.SetWaveInfo(waveNumber, activeZombies.Count);
        //waveNumber++;
    }

    public void CreateZombie()
    {
        //var point = spawnPoints[Random.Range(0, spawnPoints.Length)];
        float randX = Random.Range(-20f, 20f);
        float randZ = Random.Range(-20f, 20f);
        Vector3 randomPos = new Vector3(randX, transform.position.y, randZ);
        var zombiedata = zombieDatas[Random.Range(0, zombieDatas.Length)];
        zombiePrefab = zombiedata.modelPrefab.GetComponent<Zombie>();
        var zombie = Instantiate(zombiePrefab, randomPos, Quaternion.identity);
        //var zombie = Instantiate(zombiePrefab, spawnPoints[Random.Range(0, spawnPoints.Length)].position, Quaternion.identity);
        zombie.SetZombieData(zombiedata);
        activeZombies.Add(zombie);
        zombie.OnDeath += () => activeZombies.Remove(zombie);
        //zombie.OnDeath += () => uiManager.SetWaveInfo(waveNumber, activeZombies.Count);
        zombie.OnDeath += () => Destroy(zombie.gameObject, 5f);
    }

    private void Update()
    {
        if (activeZombies.Count == 0)
        {
            SpawnWave();
        }
    }
}
