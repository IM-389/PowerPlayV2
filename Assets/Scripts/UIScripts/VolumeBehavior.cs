using UnityEngine;
using UnityEngine.UI;

public class VolumeBehavior : MonoBehaviour
{
    [Tooltip("This contains the information for the background music.")]
    public GameObject BGM;

    [Tooltip("This contains the information for the background music.")]
    public AudioSource musicAudio;

    [Tooltip("This contains the value from the slider," +
        " and it converts to the volume.")]
    public static float sliderValue = 1;

    [Tooltip("This contains the information for the options panel.")]
    public GameObject optionsMenu;

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

        // This invokes the SetVolume function.
        SetVolume(sliderValue);

        // This looks at options panel and finds the slider BGM.
        Transform volumeObj = optionsMenu.transform.Find("BGM");

        // This gives volumeSlider the value of the slider in the 
        // options panel so it can be used.
        Slider volumeSlider = volumeObj.GetComponent<Slider>();

        // This sets the volumeSlider as the volume.
        volumeSlider.value = sliderValue;


        // This looks at options panel and finds the slider BGM.
        Transform sfxObj = optionsMenu.transform.Find("SFX Slider");

        // This gives volumeSlider the value of the slider in the 
        // options panel so it can be used.
        Slider sfxSlider = sfxObj.GetComponent<Slider>();

        // This sets the volumeSlider as the volume of the game.
        sfxSlider.value = AudioListener.volume;
    }
    /// <summary>
    /// This adjusts the volume and slider so they will
    /// be consistant and stay the same after leaving the options panel.
    /// </summary>
    /// <param name="volume">The volume of the background music.</param>
    public void SetVolume(float volume)
    {
        //This fetches the AudioSource from the GameObject.
        musicAudio = BGM.GetComponent<AudioSource>();

        // This makes the volume of the Audio match the Slider value.
        musicAudio.volume = volume;

        // The sliderValue will now be the same value
        // as the volume that was just set.
        sliderValue = volume;

        // This invokes the UpdateVolumeLabel function.
        UpdateVolumeLabel();
    }

    /// <summary>
    /// This function adjusts the text above the slider and makes it
    /// so the slider and its values will be the same
    /// even after changing the scene.
    /// </summary>
    void UpdateVolumeLabel()
    {
        // This looks at options panel and finds the BGM text.
        Transform volumeObj = optionsMenu.transform.Find("BGM Text");

        // This gives volumeText the value of the text in the 
        // options panel so it can be used.
        Text volumeText = volumeObj.GetComponent<Text>();

        // This multiplies the float to a whole number to be used
        // in the text and be easier to read.
        int percent = Mathf.RoundToInt(sliderValue * 100f);

        // This sets the text to say the percentage the volume is at.
        volumeText.text = "<b>BGM Volume:</b> " + percent + "%";

        // This looks at options panel and finds the slider BGM.
        volumeObj = optionsMenu.transform.Find("BGM");

        // This gives volumeSlider the value of the slider in the 
        // options panel so it can be used.
        Slider volumeSlider = volumeObj.GetComponent<Slider>();

        // This sets the volumeSlider as the volume so it stays
        // the same after changing scenes.
        volumeSlider.value = sliderValue;

    }

    /// <summary>
    /// This adjusts the volume and slider so they will
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
        Transform sfxObj = optionsMenu.transform.Find("SFX Text");

        // This gives volumeText the value of the text in the 
        // options panel so it can be used.
        Text volumeText = sfxObj.GetComponent<Text>();

        // This multiplies the float to a whole number to be used
        // in the text and be easier to read.
        int percent = Mathf.RoundToInt(AudioListener.volume * 100f);

        // This sets the text to say the percentage the volume is at.
        volumeText.text = "<b>SFX Volume:</b> " + percent + "%";

        // This looks at options panel and finds the SFX slider.
        sfxObj = optionsMenu.transform.Find("SFX");

        // This gives volumeSlider the value of the slider in the 
        // options panel so it can be used.
        Slider sfxSlider = sfxObj.GetComponent<Slider>();

        // This sets the volumeSlider as the volume so it stays
        // the same after changing scenes.
        sfxSlider.value = AudioListener.volume;
    }
}
