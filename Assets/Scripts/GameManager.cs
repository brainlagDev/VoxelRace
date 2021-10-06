using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public PlayerController Player;
    public PlayerController[] PlayerCars;
    public SoundManager soundManager;
    public GameObject MainUI;
    public GameObject DeathScreen;
    public GameObject LoadingScren;
    public GameObject PauseScreen;
    public GameObject OptionsScreen;
    public GameObject MugScreen;
    public Image[] Hearts;
    public Sprite FullHeart;
    public Sprite EmptyHeart;
    public Text CoinCount;
    
    
    void Awake()
    {
        //soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        Instantiate(PlayerCars[PlayerPrefs.GetInt("SelectedCar")], new Vector3(0, 0, 0), Quaternion.identity);
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        CoinCount = GameObject.Find("CoinCounter").GetComponent<Text>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Start()
    {
        soundManager.PlayAudio(soundManager.MusicClips[0]);
    }

    void Update()
    {
        for (int i = 0; i < Hearts.Length; i++)
        {
            if ((i + 1) <= Player.Lives)
            {
                Hearts[i].sprite = FullHeart;
            }
            else
            {
                Hearts[i].sprite = EmptyHeart;
            }
        }
        if(Player.Lives <= 0 && Player.isAlive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Player.isAlive = Player.isDrive = false;
            StartCoroutine(LoadDeathUI());
            //Debug.Log(Player.Lives + "(pl) <= 0");
        }
        CoinCount.text = Player.CollectedCoins.ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }

        if (!soundManager.Music.isPlaying)
        {
            soundManager.PlayNextAudio();
        }
    }
    
    public void Replay()
    {
        soundManager.PlaySound(soundManager.ButtonClick);
        LoadingScren.SetActive(true);
        StartCoroutine(LoadScene("Racing"));
    }
    public void BackToMenu()
    {
        soundManager.PlaySound(soundManager.ButtonClick);
        LoadingScren.SetActive(true);
        StartCoroutine(LoadScene("MainMenu"));
    }
    public void Pause()
    {
        soundManager.PlaySound(soundManager.ButtonClick);
        if (!PauseScreen.activeSelf)
        {
            MainUI.SetActive(false);
            PauseScreen.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            PauseScreen.SetActive(false);
            MainUI.SetActive(true);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }
    public void Options()
    {
        soundManager.PlaySound(soundManager.ButtonClick);
        if (OptionsScreen.activeSelf)
            OptionsScreen.SetActive(false);
        else 
            OptionsScreen.SetActive(true);
    }
    public void Mug()
    {
        soundManager.PlaySound(soundManager.MugSound);
        StartCoroutine(ActivateMugScreen());
    }
    IEnumerator LoadDeathUI()
    {
        Time.timeScale = 0.4f;
        if (PlayerPrefs.HasKey("Coins"))
        {
            //Debug.Log("coins = " + PlayerPrefs.GetInt("Coins") + " + " + Player.CollectedCoins);
            PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") + Player.CollectedCoins);
        }
        //else
            //Debug.Log("Key 'Coins' was not created");
        yield return new WaitForSecondsRealtime(3.0f);
        Time.timeScale = 1;
        DeathScreen.SetActive(true);
    }
    IEnumerator LoadScene(string SceneName)
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(SceneName);
        while (!asyncOperation.isDone)
            yield return null;
        //yield return new WaitForSeconds(3.0f);
    }
    IEnumerator ActivateMugScreen()
    {
        MugScreen.SetActive(true);
        yield return new WaitForSeconds(5.15f);
        MugScreen.SetActive(false);
    }
}
