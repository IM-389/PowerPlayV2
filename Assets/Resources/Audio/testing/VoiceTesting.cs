using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceTesting : MonoBehaviour
{
    public AudioClip voiceOne;
    public AudioClip voiceTwo;
    [Tooltip("This contains the information for the Voice acting.")]
    public GameObject voice;

    bool tempHelp = true;
    AudioSource voiceAudio;
    // Start is called before the first frame update
    void Start()
    {
        // This fetches the AudioSource from the GameObject.
        voiceAudio = voice.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            tempHelp = !tempHelp;
            SwitchVoice();
        }


    }

    private void SwitchVoice()
    {
        if(tempHelp)
        {
            voice.GetComponent<AudioSource>().clip = voiceOne;
        }
        else
        {
            voice.GetComponent<AudioSource>().clip = voiceTwo;
        }
        voice.GetComponent<AudioSource>().Play();
    }
}
