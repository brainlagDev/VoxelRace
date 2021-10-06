//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class PlayerFollower : MonoBehaviour
{
    public GameObject Player;
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        this.transform.position = Player.transform.position;
        this.transform.rotation = Player.transform.rotation;
    }
}
