//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class RandomGrassSpawn : MonoBehaviour
{
    public GameObject[] DecorationPrefabs;
    public float XSpawnRange = 9.5f;
    public float YSpawnRange = 5.5f;
    void Start()
    {
        for (int i = 0; i < DecorationPrefabs.Length * 3; ++i)
        {
            GameObject Grass = Instantiate(DecorationPrefabs[Random.Range(0, DecorationPrefabs.Length)], this.transform);
            Grass.transform.localPosition = new Vector3(
                Random.Range(-XSpawnRange, XSpawnRange),
                0,
                Random.Range(-YSpawnRange, YSpawnRange));
            Grass.transform.rotation = Quaternion.Euler(0, 90 * Random.Range(-4, 4), 0);
        }
    }
}
