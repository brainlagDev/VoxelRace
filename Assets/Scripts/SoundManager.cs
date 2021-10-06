using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SoundManager : MonoBehaviour
{
    public AudioSource Music;
    public AudioSource Sound;
    public AudioSource Vehicle;

    public AudioClip[] MusicClips;
    public AudioClip[] UISounds;
    public AudioClip ButtonClick;
    public AudioClip ButtonBuySound;
    public AudioClip CoinSound;
    public AudioClip HeartSound;
    public AudioClip LightSwitcherSound;
    public AudioClip MugSound;
    public AudioClip CarBeatSound;
    public AudioClip PitBeatSound;
    public AudioClip HumanDeath;

    public Slider MusicSlider;
    public Slider SoundSlider;

    private int PlayingMusicClipIndex = 0;

    void Start()
    {
        Debug.Log(Application.dataPath);
        if (!PlayerPrefs.HasKey("MusicVolume") || !PlayerPrefs.HasKey("SoundVolume"))
        {
            PlayerPrefs.SetFloat("MusicVolume", 0.25f);
            PlayerPrefs.SetFloat("SoundVolume", 0.25f);
        }
        //Music = GetComponent<AudioSource>();
        //Sound = GetComponent<AudioSource>();
        Music.volume = PlayerPrefs.GetFloat("MusicVolume");
        Sound.volume = PlayerPrefs.GetFloat("SoundVolume");

        MusicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        SoundSlider.value = PlayerPrefs.GetFloat("SoundVolume");
        Vehicle.volume = Sound.volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            int index = Random.Range(0, MusicClips.Length);
            PlayingMusicClipIndex = index;
            PlayAudio(MusicClips[index], false);
        }
        else if (Input.GetKeyDown(KeyCode.Q))
        {
            Music.Stop();
            PlayPreviousAudio();
        }
        else if (Input.GetKeyDown(KeyCode.E))
        {
            Music.Stop();
            PlayNextAudio();
        }
    }
    public void PlayNextAudio()
    {
        if (PlayingMusicClipIndex + 1 < MusicClips.Length)
        {
            PlayAudio(MusicClips[PlayingMusicClipIndex + 1], false);
            PlayingMusicClipIndex++;
        }
        else
        {
            PlayAudio(MusicClips[0], false);
            PlayingMusicClipIndex = 0;
        }
    }
    public void PlayPreviousAudio()
    {
        if (PlayingMusicClipIndex - 1 >= 0)
        {
            PlayAudio(MusicClips[PlayingMusicClipIndex - 1], false);
            PlayingMusicClipIndex--;
        }
        else
        {
            PlayAudio(MusicClips[MusicClips.Length - 1], false);
            PlayingMusicClipIndex = MusicClips.Length - 1;
        }
    }
    public void PlayAudio(AudioClip audioClip, bool PlayWithDelay = false)
    {
        Music.Stop();
        if (!PlayWithDelay)
            Music.PlayOneShot(audioClip);
        else
            StartCoroutine(PlayMusicWithDelay(Music, audioClip));
    }
    public void PlaySound(AudioClip audioClip, bool StopOtherSounds = true)
    {
        if (StopOtherSounds)
            Sound.Stop();
        Sound.PlayOneShot(audioClip);
    }
    public void PlayVehicleSound(AudioClip audioClip)
    {
        Vehicle.Stop();
        Vehicle.PlayOneShot(audioClip);
    }
    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        Music.volume = value;
    }
    public void SetSoundVolume(float value)
    {
        PlayerPrefs.SetFloat("SoundVolume", value);
        Sound.volume = value;
        Vehicle.volume = value;
    }
    IEnumerator PlayMusicWithDelay(AudioSource audioSource, AudioClip audioClip)
    {
        yield return new WaitForSeconds(5.0f);
        audioSource.PlayOneShot(audioClip);
    }
}
