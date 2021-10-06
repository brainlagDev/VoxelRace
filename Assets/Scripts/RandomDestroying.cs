using System.Collections;
using UnityEngine;

public class RandomDestroying : MonoBehaviour
{
    public float ChanceOfStaying = 50.0f;
    void Start()
    {
        if (Random.Range(0, 100) > ChanceOfStaying)
        {
            Destroy(this.gameObject);
        }
    }
}
