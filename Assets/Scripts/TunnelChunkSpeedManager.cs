using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelChunkSpeedManager : MonoBehaviour
{
    const float MinSpeed = 4.0f;
    const float MaxSpeed = 7.0f;
    public float CarsSpeed;
    void Awake()
    {
        CarsSpeed = MinSpeed + TimeCounter.Minutes + Random.Range(1, 3);
        if (CarsSpeed > MaxSpeed)
            CarsSpeed = MaxSpeed;
    }
}
