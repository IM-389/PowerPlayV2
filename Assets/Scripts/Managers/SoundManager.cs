using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    // These control the volume. The basic mix between sound types has been 
    // done already, but these allow for in-game tuning
    FMOD.Studio.Bus Master;
    [Header("Volume Buses")]
    public string masterBus;
    float masterVolume = 1f;

    FMOD.Studio.Bus Music;
    public string musicBus;
    float musicVolume = 1f;

    FMOD.Studio.Bus SFX;
    public string sfxBus;
    float sfxVolume = 1f;

    [Header("Volume Sliders")]
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public Text masterSliderText;
    public Text musicSliderText;
    public Text sfxSliderText;

    // These are the locations of the sound events
    [Header("Sound Locations")]
    [FMODUnity.EventRef]
    public string buttonPress;
    [FMODUnity.EventRef]
    public string quizAffirmative;
    [FMODUnity.EventRef]
    public string quizNegative;


    // Start is called before the first frame update
    void Start()
    {
        // Initializing the volume buses
        Master = RuntimeManager.GetBus(masterBus);
        Music = RuntimeManager.GetBus(musicBus);
        SFX = RuntimeManager.GetBus(sfxBus);

        LoadSettings();
        SetSliderText();

        masterSlider.value = masterVolume * 100;
        musicSlider.value = musicVolume * 100;
        sfxSlider.value = sfxVolume * 100;
        
    }

    // Update is called once per frame
    void Update()
    {
        Master.setVolume(masterVolume);
        Music.setVolume(musicVolume);
        SFX.setVolume(sfxVolume);
    }

    /// <summary>
    /// Handles the Master Volume for all sounds
    /// </summary>
    /// <param name="newVolume"></param>
    public void MasterVolumeLevel (float newVolume)
    {
        masterVolume = newVolume / 100;
        SetSliderText();
    }

    /// <summary>
    /// Handles the volume level of all music and backgrounds
    /// </summary>
    /// <param name="newVolume"></param>
    public void MusicVolumeLevel (float newVolume)
    {
        musicVolume = newVolume / 100;
        SetSliderText();
    }

    /// <summary>
    /// Handles the volume of all sound effects
    /// </summary>
    /// <param name="newVolume"></param>
    public void SFXVolumeLevel (float newVolume)
    {
        sfxVolume = newVolume / 100;
        SetSliderText();
    }

    /// <summary>
    /// Plays a test sound after change in SFX volume
    /// </summary>
    public void TestSound()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sound Effects/Griddy Affirmative");
    }

    /// <summary>
    /// Plays the button press sound
    /// </summary>
    public void ButtonClick()
    {
        RuntimeManager.PlayOneShot(buttonPress);
    }
    
    void SetSliderText()
    {
        masterSliderText.text = "Master = " + (Mathf.Round(masterSlider.value)) + "%";
        musicSliderText.text = "Music = " + (Mathf.Round(musicSlider.value)) + "%";
        sfxSliderText.text = "SFX = " + (Mathf.Round(sfxSlider.value)) + "%";
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("Master Volume"))
        {
            masterVolume = PlayerPrefs.GetFloat("Master Volume");
        }
        else
        {
            masterVolume = 1f;
        }

        if (PlayerPrefs.HasKey("Music Volume"))
        {
            musicVolume = PlayerPrefs.GetFloat("Music Volume");
        }
        else
        {
            musicVolume = 0.5f;
        }

        if (PlayerPrefs.HasKey("SFX Volume"))
        {
            sfxVolume = PlayerPrefs.GetFloat("SFX Volume");
        }
        else
        {
            sfxVolume = 0.5f;
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("Master Volume", masterVolume);
        PlayerPrefs.SetFloat("Music Volume", musicVolume);
        PlayerPrefs.SetFloat("SFX Volume", sfxVolume);
    }

    private void OnDisable()
    {
        SaveSettings();
    }
}
