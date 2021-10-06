using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public PlayerController[] Cars;
    //public GameObject MainMenuScreen;
    public SoundManager soundManager;
    public GameObject LoadingRacingScreen;
    public GameObject OptionsScreen;
    public Transform VehicleTransform;
    public Text CoinCounter;

    public GameObject PlayButton;
    public GameObject BuyButton;
    public GameObject LockedButton;

    public GameObject[] Lights;

    public int[] PricesForCars;
    public int SelectedCarIndex;
    
    void Start()
    {
        if (!PlayerPrefs.HasKey("Cars"))
            PlayerPrefs.SetInt("Cars", 0);
        else
            SelectedCarIndex = PlayerPrefs.GetInt("SelectedCar");

        PlayerController temp = Instantiate(Cars[SelectedCarIndex], VehicleTransform.position, VehicleTransform.rotation, VehicleTransform);
        temp.name = "Car";
        temp.GetComponent<PlayerController>().enabled = false;

        if (!PlayerPrefs.HasKey("Coins"))
        {
            Debug.Log("Player prefs coins was created firstly");
            PlayerPrefs.SetInt("Coins", 0);
        }
        //Debug.Log("Player coins: " + PlayerPrefs.GetInt("Coins").ToString());
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        soundManager.Music.volume = PlayerPrefs.GetFloat("MusicVolume");
        soundManager.PlayAudio(soundManager.MusicClips[0]);
    }

    void Update()
    {
        //if (Input.GetKey(KeyCode.C))                        // for debugging
        //{
        //    PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 10);
        //}
        //if(Input.GetKey(KeyCode.D))                         // for debugging
        //{
        //    PlayerPrefs.SetInt("Cars", 0);
        //}

        CoinCounter.text = PlayerPrefs.GetInt("Coins").ToString();

        //  Changing main button
        if (SelectedCarIndex <= PlayerPrefs.GetInt("Cars"))
        {
            PlayButton.SetActive(true);
            BuyButton.SetActive(false);
            LockedButton.SetActive(false);
        }
        else if (SelectedCarIndex == PlayerPrefs.GetInt("Cars") + 1)
        {
            PlayButton.SetActive(false);
            BuyButton.SetActive(true);
            LockedButton.SetActive(false);

            BuyButton.GetComponentInChildren<Text>().text = PricesForCars[SelectedCarIndex].ToString();
        }
        else if (SelectedCarIndex > PlayerPrefs.GetInt("Cars") + 1)
        {
            PlayButton.SetActive(false);
            BuyButton.SetActive(false);
            LockedButton.SetActive(true);
        }
    }

    public void Play()
    {
        PlayerPrefs.SetInt("SelectedCar", SelectedCarIndex);
        GameObject.Find("Garage").SetActive(false);
        LoadingRacingScreen.SetActive(true);
        StartCoroutine(LoadScene());
        soundManager.PlaySound(soundManager.ButtonClick);
    }
    public void BuyCar()
    {
        if (PricesForCars[SelectedCarIndex] <= PlayerPrefs.GetInt("Coins"))
        {
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - PricesForCars[SelectedCarIndex]);
            PlayerPrefs.SetInt("Cars", PlayerPrefs.GetInt("Cars") + 1);
            soundManager.PlaySound(soundManager.ButtonBuySound);
        }
    }
    public void SwitchCar(bool side)
    {
        if (side)  // right side
        {
            if (SelectedCarIndex + 1 < Cars.Length)
            {
                SelectedCarIndex++;
            }
        }
        else       // left side
        {
            if (SelectedCarIndex - 1 >= 0)
            {
                SelectedCarIndex--;
                
            }
        }
        soundManager.PlaySound(soundManager.ButtonClick);
        Destroy(GameObject.Find("Car"));
        PlayerController temp = Instantiate(Cars[SelectedCarIndex], VehicleTransform.position, VehicleTransform.rotation, VehicleTransform);
        temp.name = "Car";
        //temp.transform.localScale = VehicleTransform.localScale;
        temp.GetComponent<PlayerController>().enabled = false;
    }
    public void Options()
    {
        if (OptionsScreen.activeSelf)
        {
            OptionsScreen.SetActive(false);
            //MainMenuScreen.SetActive(true);
        }
        else
        {
            OptionsScreen.SetActive(true);
            //MainMenuScreen.SetActive(false);
        }
        soundManager.PlaySound(soundManager.ButtonClick);
    }
    public void SwitchLight()
    {
        if (Lights[0].activeSelf)
        {
            foreach (var light in Lights)
                light.SetActive(false);
        }
        else
        {
            foreach (var light in Lights)
                light.SetActive(true);
        }
        soundManager.PlaySound(soundManager.LightSwitcherSound);
    }
    public  void Exit()
    {
        //SceneManager.UnloadSceneAsync("MainMenu");  // temporary, because Application.Quit() doesn't work
        soundManager.PlaySound(soundManager.ButtonClick);
        Application.Quit();
    }

    // debugging

    public void IncreaseCoins()
    {
        PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + 100);
    }
    public void ClearCars()
    {
        PlayerPrefs.SetInt("Cars", 0);
    }
    IEnumerator LoadScene()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("Racing");
        while (!asyncOperation.isDone)
            yield return null;
        //yield return new WaitForSeconds(3.0f);
    }
}
