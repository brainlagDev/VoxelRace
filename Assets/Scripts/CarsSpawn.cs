using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class CarsSpawn : MonoBehaviour
{
    public Transform Spawn;
    public Car[] CarsPrefabs;
    public TunnelChunkSpeedManager SpeedManager;
    public float MinSpawnTime = 15.0f;
    public float MaxSpawnTime = 30.0f;
    public bool SpawnCars = true;
    public bool TrackChunk = false;
    public bool CarIsDetected = false;

    private void Start()
    {
        //SpeedManager = transform.  //GetComponent<TunnelChunkSpeedManager>();
        if (SpawnCars)
            StartCoroutine(SpawnCar());
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("AnotherCar") || other.gameObject.tag.Equals("Player"))
        {
            CarIsDetected = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("AnotherCar") || other.gameObject.tag.Equals("Player"))
        {
            CarIsDetected = false;
        }
    }

    IEnumerator SpawnCar(bool WaitForRandomTime = true)
    {
        if (GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().isAlive)
        {
            if (CarIsDetected)
            {
                StopCoroutine(SpawnCar());
                yield return new WaitForSeconds(1.0f);
                StartCoroutine(SpawnCar());
            }
            else
            {
                Car NewCar = Instantiate(CarsPrefabs[Random.Range(0, CarsPrefabs.Length)], Spawn);
                NewCar.transform.position = Spawn.transform.position;
                NewCar.transform.rotation = Spawn.transform.rotation;
                NewCar.Speed = SpeedManager.CarsSpeed;
                if (TrackChunk)
                    NewCar.TrackCar = true;
                float MinTime = MinSpawnTime * 0.5f * TimeCounter.Minutes;
                if (MinTime <= 5.0f)
                    MinTime = 5.0f;
                //float Seconds = Random.Range(MinTime, MaxSpawnTime);
                //if (WaitForRandomTime)
                    yield return new WaitForSeconds(Random.Range(MinTime, MaxSpawnTime));
                StartCoroutine(SpawnCar());
            }
        }
        else
            StopCoroutine(SpawnCar());
    }
}
