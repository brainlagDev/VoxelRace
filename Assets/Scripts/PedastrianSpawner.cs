using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PedastrianSpawner : MonoBehaviour
{
    public Transform[] Spawners;
    public Pedastrian[] Pedastrians;
    public float MinSpawnTime;
    public float MaxSpawnTime;
    
    void Start()
    {
        StartCoroutine(SpawnPedastrian());
    }

    IEnumerator SpawnPedastrian()
    {
        Pedastrian pedastrian = Instantiate(Pedastrians[Random.Range(0, Pedastrians.Length)], 
            Spawners[Random.Range(0, Spawners.Length)]);
        yield return new WaitForSeconds(Random.Range(MinSpawnTime, MaxSpawnTime));
        StartCoroutine(SpawnPedastrian());
    }
}