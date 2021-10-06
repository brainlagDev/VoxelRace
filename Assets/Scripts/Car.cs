using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public float Speed = 5.0f;
    public float RotationSpeed = 2.0f;
    public float TargetRotation;
    public int ChanceOfTurn = 25;
    public bool TrackCar = false;
    private bool FirstRotate = true;
    private bool isAbleToRotate = true;

    private void Start()
    {
        TargetRotation = (int)this.transform.localRotation.y;
        if (TrackCar)
        {
            FirstRotate = false;
            ChanceOfTurn = 50;
        }
    }
    void Update()
    {
        this.transform.Translate(Vector3.forward * Speed * Time.deltaTime);

        if (this.transform.localRotation.y < TargetRotation)
        {
            this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.Euler(0, TargetRotation, 0), RotationSpeed * Time.deltaTime);
        }
        else if (this.transform.localRotation.y > TargetRotation)
        {
            this.transform.localRotation = Quaternion.Euler(0, TargetRotation, 0);
            TargetRotation = this.transform.localRotation.y;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Equals("LastCarTurn"))
        {
            if (isAbleToRotate)
            {
                //Debug.Log(this.gameObject.name + ": LCT, TR = " + (int)this.transform.localRotation.y + " + 90 = " + TargetRotation);
                TargetRotation += 90;
                StartCoroutine(TurnWaiting());
            }
        }
        else if (other.gameObject.name.Equals("CarTurn"))
        {
            if (isAbleToRotate)
            {
                int Chance = Random.Range(0, 100);
                if (FirstRotate)
                {
                    Chance = 0;
                    FirstRotate = false;
                    StartCoroutine(TurnWaiting());
                }
                if (Chance <= ChanceOfTurn)
                {
                    TargetRotation += 90;
                    StartCoroutine(TurnWaiting());
                    //Debug.Log(this.transform.localRotation.y + " + 90.0f" + " = " + TargetRotation);
                }
            }
        }
        else if (other.gameObject.name.Equals("CarDestroy") || other.gameObject.name.Equals("AnotherCar"))
        {
            Destroy(this.gameObject);
        }
    }

    //Ie
    IEnumerator TurnWaiting()
    {
        if (TrackCar)
        {
            isAbleToRotate = false;
            yield return new WaitForSeconds(2.0f);
            isAbleToRotate = true;
        }
    }
}
