using UnityEngine;

public class CoinRotation : MonoBehaviour
{
    void FixedUpdate()
    {
        this.transform.rotation *= Quaternion.Euler(0, -1.8f, 0);
    }
}
