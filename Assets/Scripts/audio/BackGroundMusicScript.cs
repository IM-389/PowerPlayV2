using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusicScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {

        gameObject.GetComponent<AudioSource>().mute = SoundManager.backGroundMusic;
    }


    public void toggleBackGroundMusic()
    {
        SoundManager.backGroundMusic = !SoundManager.backGroundMusic; ;
    }

}
