using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VideoSettingsBehaviour : MonoBehaviour
{

    [Tooltip("Dropdown for selecting the resolution")]
    public TMP_Dropdown resolutionSelect;

    [Tooltip("Toggle used for detecting fullscreen status")]
    public Toggle fullscreenToggle;
    
    public Resolution[] resolutions;

    private Dictionary<int, int> resolutionIndexMap = new Dictionary<int, int>();
    
    // Start is called before the first frame update
    void Start()
    {
        resolutionSelect.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResIndex = 0;
        int resMapping = 0;
        
        for (int i = 0; i < resolutions.Length; ++i)
        {
            string option = $"{resolutions[i].width} x {resolutions[i].height}";
            if (!options.Contains(option))
            {
                options.Add(option);
                resolutionIndexMap.Add(resMapping, i);
                ++resMapping;
            }

            if (resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = i;
            }
        }
        
        resolutionSelect.AddOptions(options);
        resolutionSelect.RefreshShownValue();
        LoadSettings(currentResIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        //Screen.fullScreen = fullscreenToggle.isOn;
        Screen.fullScreen = isFullscreen;
        SaveSettings();
    }

    public void SetResolution(int index)
    {
        Resolution resolution = resolutions[resolutionIndexMap[resolutionSelect.value]];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        SaveSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("ResolutionIndex", resolutionSelect.value);
        PlayerPrefs.SetInt("IsFullscreen", Convert.ToInt32(Screen.fullScreen));
    }

    public void LoadSettings(int currentResIndex)
    {
        if (PlayerPrefs.HasKey("ResolutionIndex"))
        {
            resolutionSelect.value = PlayerPrefs.GetInt("ResolutionIndex");
        }
        else
        {
            resolutionSelect.value = currentResIndex;
        }

        if (PlayerPrefs.HasKey("IsFullscreen"))
        {
            bool full = Convert.ToBoolean(PlayerPrefs.GetInt("IsFullscreen"));
            Screen.fullScreen = full;
            fullscreenToggle.isOn = full;
            //fullscreenToggle.onValueChanged.Invoke(full);
        }
        else
        {
            Screen.fullScreen = true;
        }
    }

    private void OnDisable()
    {
        SaveSettings();
    }
}
