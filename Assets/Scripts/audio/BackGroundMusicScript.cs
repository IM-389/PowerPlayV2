using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusicScript : MonoBehaviour
{

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
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
