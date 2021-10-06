using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelLoadingAnimation : MonoBehaviour
{
    void FixedUpdate()
    {
        this.transform.rotation *= Quaternion.Euler(0, 0, -3.6f);
    }
}
