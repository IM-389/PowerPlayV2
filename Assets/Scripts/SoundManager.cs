using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class SoundManager : MonoBehaviour
{
    FMOD.Studio.Bus Master;
    public string masterBus;
    float masterVolume = 1f;

    FMOD.Studio.Bus Music;
    public string musicBus;
    float musicVolume = 0.5f;

    FMOD.Studio.Bus SFX;
    public string sfxBus;
    float sfxVolume = 0.5f;


    // Start is called before the first frame update
    void Start()
    {
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

    public void MasterVolumeLevel (float newVolume)
    {
        masterVolume = newVolume;
    }
    public void MusicVolumeLevel (float newVolume)
    {
        musicVolume = newVolume;
    }
    public void SFXVolumeLevel (float newVolume)
    {
        sfxVolume = newVolume;
        FMODUnity.RuntimeManager.PlayOneShot("event:/Sound Effects/Griddy Affirmative");
    }
}
