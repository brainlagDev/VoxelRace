using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    // Constructor

    public PlayerController() { }

    // PARAMETERS


    [Range(1, 5)] public int MOBILITY;
    [Range(1, 5)] public int STOPPING_POWER;
    [Range(1, 3)] public int LIVES;

    public int Lives;
    public int CollectedCoins = 0;
    public float RotationSpeed;
    public float DriveSpeed;
    public float TargetSpeed;
    public float Acceleration = 0.25f;
    public const float MaxSpeed = 20.0f;
    public const float MinSpeed = 5.45f;

    public bool isDrive = true;
    public bool isAlive = true;
    public bool isAccelerating = false;
    public bool isSlowingDown = false;
    private bool isAbleToSlowDown = true;
    private bool isAbleToGetDamage = true;

    SoundManager soundManager;
    public AudioClip RidingSound;

    Rigidbody RigidbodyComponent;
    Animation AnimationComponent;
    CharacterController CharacterControllerComponent;
    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        Lives = LIVES;
        RigidbodyComponent = this.GetComponent<Rigidbody>();
        AnimationComponent = this.GetComponent<Animation>();
        CharacterControllerComponent = this.GetComponent<CharacterController>();
        StartCoroutine(IncreaseSpeed());
    }

    void Update()
    {
        if (isAlive)
        {
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    if (isDrive == true)
            //        isDrive = false;
            //    else
            //        isDrive = true;
            //}

            // Increasing speed / temporaty accelerating
            if (Input.GetKey(KeyCode.W))
            {
                isAccelerating = true;
                float maxspeed = TargetSpeed * (1 + (MOBILITY / 5.0f));
                if (DriveSpeed < maxspeed)
                    DriveSpeed += MOBILITY * 2 * Time.deltaTime;
                if (soundManager.Vehicle.pitch < 1.25f)
                    soundManager.Vehicle.pitch+=0.005f;
            }
            if (Input.GetKeyUp(KeyCode.W))
            {
                isAccelerating = false;
            }
            if (!isAccelerating && soundManager.Vehicle.pitch > 1.0f)
                soundManager.Vehicle.pitch -= 0.05f;

            // Moving forward and Rotating
            if (isDrive)
            {
                RigidbodyComponent.velocity = transform.forward * DriveSpeed;
                //RigidbodyComponent.
                //CharacterControllerComponent.
            }

            // Rotation
            float speed = MOBILITY * 0.5f + DriveSpeed * 0.5f;
            transform.Rotate(0, 
                Input.GetAxis("Horizontal") * (RotationSpeed * speed) * Time.deltaTime,
                0);

            // Slowing down
            if (Input.GetKey(KeyCode.Space) && isAbleToSlowDown)
            {
                // I way
                //StartCoroutine(SlowDown());

                // II way
                isSlowingDown = true;
                DriveSpeed -= STOPPING_POWER * 5 * Time.deltaTime;
                if (DriveSpeed < MinSpeed)
                    DriveSpeed = MinSpeed;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                StartCoroutine(BreakRefresh());
            }

            // Getting target speed
            if (!isSlowingDown && !isAccelerating && DriveSpeed != TargetSpeed)
            {
                if (DriveSpeed > TargetSpeed - 0.5f && DriveSpeed < TargetSpeed + 0.5f)
                {
                    DriveSpeed = TargetSpeed;
                }

                if (DriveSpeed < TargetSpeed)
                    DriveSpeed += MOBILITY * 5.0f * Time.deltaTime;
                else if (DriveSpeed > TargetSpeed)
                    DriveSpeed -= MOBILITY * 5.0f * Time.deltaTime;
            }

            if (!soundManager.Vehicle.isPlaying)
            {
                soundManager.Vehicle.Stop();
                soundManager.PlayVehicleSound(RidingSound);
            }
        }
        else
        {
            //soundManager.Vehicle.Stop();
            if (soundManager.Vehicle.pitch > 0)
                soundManager.Vehicle.pitch -= 0.01f;
        }
    }

    void OnTriggerEnter(Collider Object)
    {
        if (Object.gameObject.tag.Equals("Obstruction"))
        {
            StartCoroutine(GetDamage());
            soundManager.PlaySound(soundManager.PitBeatSound, false);
        }    
        else if (Object.gameObject.tag.Equals("Coin"))
        {
            //++CoinCollect.Coins;
            ++CollectedCoins;
            StartCoroutine(SlowScale(Object.gameObject));
            soundManager.PlaySound(soundManager.CoinSound, false);
        }
        else if (Object.gameObject.tag.Equals("Heart"))
        {
            if (Lives + 1 <= 3)
            {
                soundManager.PlaySound(soundManager.HeartSound, false);
                ++Lives;
            }
            else
            {
                soundManager.PlaySound(soundManager.CoinSound, false);
                CollectedCoins++;
            }
            StartCoroutine(SlowScale(Object.gameObject));
        }
        else if (Object.gameObject.tag.Equals("AnotherCar") || Object.gameObject.tag.Equals("Enclosure"))
        {
            soundManager.PlaySound(soundManager.CarBeatSound, false);
            //StartCoroutine(Death());

            // II way

            RigidbodyComponent.constraints = RigidbodyConstraints.None;
            RigidbodyComponent.AddForce(Vector3.up * 25000.0f, ForceMode.Impulse);
            RigidbodyComponent.AddTorque(new Vector3
                (Random.Range((int)-1, (int)1), 
                Random.Range((int)-1, (int)1), 
                Random.Range((int)-1, (int)1)) * 2000, 
                ForceMode.Impulse);
            Lives = 0;
        }
        else if (Object.gameObject.tag.Equals("Mug"))
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().Mug();
            Destroy(Object.gameObject);
        }
    }

    IEnumerator SlowScale(GameObject Object)
    {
        Object.gameObject.GetComponent<BoxCollider>().enabled = false;
        while (Object.transform.localScale.x > 0.3f)
        {
            Object.transform.localScale = new Vector3(Object.transform.localScale.x - 0.05f, Object.transform.localScale.y - 0.05f, Object.transform.localScale.z - 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        if (Object.gameObject != null)
            Destroy(Object);
    }

    IEnumerator BreakRefresh()
    {
        isSlowingDown = false;
        isAbleToSlowDown = false;
        yield return new WaitForSeconds(5.5f - STOPPING_POWER);
        isAbleToSlowDown = true;
    }

    IEnumerator IncreaseSpeed()
    {
        yield return new WaitForSeconds(15.0f);
        TargetSpeed += Acceleration;
        if (isAlive)
        {
            if (TargetSpeed <= MaxSpeed)
            {
                StartCoroutine(IncreaseSpeed());
                if (TargetSpeed >= MaxSpeed * 0.75f)
                    Acceleration *= 0.75f;
            }
            else
                TargetSpeed = MaxSpeed;
        }
    }

    IEnumerator GetDamage()
    {
        if(isAbleToGetDamage)
            Lives--;
        isAbleToGetDamage = false;
        yield return new WaitForSeconds(3.0f);
        isAbleToGetDamage = true;
    }
}   