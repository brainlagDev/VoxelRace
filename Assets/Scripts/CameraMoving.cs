using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    public Vector3 CameraPositionOffset = new Vector3();
    public Transform Target;
    public bool FollowGameObjectWithTagPlayer = false;
    void Start()
    {
        if (FollowGameObjectWithTagPlayer)
        {
            GameObject Player = GameObject.FindGameObjectWithTag("Player");
            Target = Player.transform;
        }
    }

    void Update()
    {
        this.transform.position = new Vector3(
            Target.transform.position.x + CameraPositionOffset.x,
            Target.transform.position.y + CameraPositionOffset.y,
            Target.transform.position.z + CameraPositionOffset.z);
    }
}
