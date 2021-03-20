using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIBehaviour : MonoBehaviour
{
    bool menuOpen = false;
    public GameObject pause;
    public TimeManager tm;

    [Tooltip("Used to prevent building while paused")]
    public GameObject pauseBlocker;
    // Start is called before the first frame update
    void Start()
    {
        tm = GameObject.FindObjectOfType<TimeManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!menuOpen)
            {
                menuOpen = true;
                pause.SetActive(true);
                pauseBlocker.SetActive(true);
                Time.timeScale = 0;
            }
            else if(menuOpen && pause.activeSelf)
            {
                Unpause();
            }
            
        }
    }

    public void Unpause()
    {
        menuOpen = false;
        pause.SetActive(false);
        pauseBlocker.SetActive(false);
        ResumeTime();
    }
    
    public void ResumeTime()
    {
        Time.timeScale = tm.resume;
    }

    public void LoadScene(int id)
    {
        SceneManager.LoadScene(id);
    }
    
}
