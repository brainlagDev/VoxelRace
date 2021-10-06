//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

public class Pedastrian : MonoBehaviour
{
    public GameObject DeadBody;
    public GameObject Coin;
    public float Speed;
    public bool isAlive = true;
    public bool isDead = false;

    void Update()
    {
        if (isAlive)
            this.transform.Translate(0, 0, Speed * Time.deltaTime);         
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("PedastrianDestroy"))
        {
            Destroy(this.gameObject);
        }
        else if (other.gameObject.tag.Equals("Player"))
        {
            if (isAlive)
            {
                var sound = GameObject.Find("SoundManager").GetComponent<SoundManager>();
                sound.PlaySound(sound.HumanDeath, false);
                isAlive = false;
                GameObject deadbody = Instantiate(DeadBody, this.transform.position, this.transform.rotation);
                //GameObject NewCoin = Instantiate(Coin, this.transform.position, this.transform.rotation);
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().CollectedCoins++;
                this.gameObject.SetActive(false);
            }
        }
    }
}
