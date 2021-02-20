using System;
using UnityEngine;
using UnityEngine.UI;

public class VolumeBehavior : MonoBehaviour
{
    [Tooltip("This contains the information for the background music object.")]
    public GameObject BGM;

    [Tooltip("This contains the information for the background music AudioSource.")]
    public AudioSource musicAudio;

    [Tooltip("This contains the information for the Voice acting object.")]
    public GameObject voice;

    [Tooltip("This contains the information for the current voice clip.")]
    AudioSource voiceAudio;

    [Tooltip("This contains the value from the slider and it converts to the volume.")]
    public static float sliderValue = 0.8f;

    [Tooltip("This contains the information for the Volume panel.")]
    public GameObject volumeMenu;

    // Start is called before the first frame update
    void Start()
    {
        // This makes it so the background music
        // isn't affected by the master volume.
        musicAudio.ignoreListenerVolume = true;

        // This fetches the AudioSource from the GameObject.
        musicAudio = BGM.GetComponent<AudioSource>();

        // This plays the AudioClip attached to the AudioSource on startup.
        musicAudio.Play();

        // This invokes the SetMusic function.
        SetMusic(sliderValue);

        // ----------------------------------------

        // This invokes the SetSFX function.
        SetSFX(sliderValue);

        // ----------------------------------------

        // This fetches the AudioSource from the GameObject.
        voiceAudio = voice.GetComponent<AudioSource>();

        // This makes it so the voice isn't affected by the master volume.
        voiceAudio.ignoreListenerVolume = true;

        // This plays the AudioClip attached to the AudioSource on startup.
        voiceAudio.Play();

        // This invokes the SetVoice function.
        SetVoice(sliderValue);

    }

    /// <summary>
    /// This adjusts the voice volume and slider so they will
    /// be consistant and stay the same after leaving the options panel.
    /// </summary>
    /// <param name="volume">The volume of the background music.</param>
    public void SetVoice(float volume)
    {

        // This makes the volume of the Audio match the Slider value.
        voiceAudio.volume = volume;

        // The sliderValue will now be the same value
        // as the volume that was just set.
        sliderValue = volume;

        // This invokes the UpdateVoiceLabel function.
        UpdateVoiceLabel();
    }
    /// <summary>
    /// This function adjusts the text above the slider and makes it
    /// so the slider and its values will be the same
    /// even after changing the scene.
    /// </summary>
    private void UpdateVoiceLabel()
    {
        // This looks at options panel and finds the Voice Text.
        Transform voiceObj = volumeMenu.transform.Find("Voice Text");

        // This gives voiceText the value of the text in the 
        // options panel so it can be used.
        Text voiceText = voiceObj.GetComponent<Text>();

        // This multiplies the float to a whole number to be used
        // in the text and be easier to read.
        int percent = Mathf.RoundToInt(sliderValue * 100f);

        // This sets the text to say the percentage the volume is at.
        voiceText.text = "<b>Voice Volume:</b> " + percent + "%";

        // This looks at options panel and finds the slider VOICE.
        voiceObj = volumeMenu.transform.Find("VOICE");

        // This gives volumeSlider the value of the slider in the 
        // options panel so it can be used.
        Slider volumeSlider = voiceObj.GetComponent<Slider>();

        // This sets the volumeSlider as the volume so it stays
        // the same after changing scenes.
        volumeSlider.value = sliderValue;
    }

    /// <summary>
    /// This adjusts the music volume and slider so they will
    /// be consistant and stay the same after leaving the options panel.
    /// </summary>
    /// <param name="volume">The volume of the background music.</param>
    public void SetMusic(float volume)
    {

        // This makes the volume of the Audio match the Slider value.
        musicAudio.volume = volume;

        // The sliderValue will now be the same value
        // as the volume that was just set.
        sliderValue = volume;

        // This invokes the UpdateVolumeLabel function.
        UpdateMusicLabel();
    }

    /// <summary>
    /// This function adjusts the text above the slider and makes it
    /// so the slider and its values will be the same
    /// even after changing the scene.
    /// </summary>
    void UpdateMusicLabel()
    {
        // This looks at options panel and finds the BGM text.
        Transform volumeObj = volumeMenu.transform.Find("BGM Text");

        // This gives volumeText the value of the text in the 
        // options panel so it can be used.
        Text volumeText = volumeObj.GetComponent<Text>();

        // This multiplies the float to a whole number to be used
        // in the text and be easier to read.
        int percent = Mathf.RoundToInt(sliderValue * 100f);

        // This sets the text to say the percentage the volume is at.
        volumeText.text = "<b>BGM Volume:</b> " + percent + "%";

        // This looks at options panel and finds the slider BGM.
        volumeObj = volumeMenu.transform.Find("BGM");

        // This gives volumeSlider the value of the slider in the 
        // options panel so it can be used.
        Slider volumeSlider = volumeObj.GetComponent<Slider>();

        // This sets the volumeSlider as the volume so it stays
        // the same after changing scenes.
        volumeSlider.value = sliderValue;

    }

    /// <summary>
    /// This adjusts the SFX volume and slider so they will
    /// be consistant and stay the same after leaving the options panel.
    /// </summary>
    /// <param name="volume">The volume of the background music.</param>
    public void SetSFX(float volume)
    {
        // The game volume will now be the same value
        // as the volume that was just set.
        AudioListener.volume = volume;
        UpdateSFXLabel();
    }

    /// <summary>
    /// This function adjusts the text above the slider and makes it
    /// so the slider and its values will be the same
    /// even after changing the scene.
    /// </summary>
    void UpdateSFXLabel()
    {
        // This looks at options panel and finds the SFX text.
        Transform sfxObj = volumeMenu.transform.Find("SFX Text");

        // This gives volumeText the value of the text in the 
        // options panel so it can be used.
        Text volumeText = sfxObj.GetComponent<Text>();

        // This multiplies the float to a whole number to be used
        // in the text and be easier to read.
        int percent = Mathf.RoundToInt(AudioListener.volume * 100f);

        // This sets the text to say the percentage the volume is at.
        volumeText.text = "<b>SFX Volume:</b> " + percent + "%";

        // This looks at options panel and finds the SFX slider.
        sfxObj = volumeMenu.transform.Find("SFX");

        // This gives volumeSlider the value of the slider in the 
        // options panel so it can be used.
        Slider sfxSlider = sfxObj.GetComponent<Slider>();

        // This sets the volumeSlider as the volume so it stays
        // the same after changing scenes.
        sfxSlider.value = AudioListener.volume;
    }
}
