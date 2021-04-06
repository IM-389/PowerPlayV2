using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

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
    float musicVolume = 0.5f;

    FMOD.Studio.Bus SFX;
    public string sfxBus;
    float sfxVolume = 0.5f;

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
    }

    /// <summary>
    /// Handles the volume level of all music and backgrounds
    /// </summary>
    /// <param name="newVolume"></param>
    public void MusicVolumeLevel (float newVolume)
    {
        musicVolume = newVolume / 100;
    }

    /// <summary>
    /// Handles the volume of all sound effects
    /// </summary>
    /// <param name="newVolume"></param>
    public void SFXVolumeLevel (float newVolume)
    {
        sfxVolume = newVolume / 100;
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
}
