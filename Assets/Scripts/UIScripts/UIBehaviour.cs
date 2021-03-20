using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBehaviour : MonoBehaviour
{
    bool menuOpen = false;
    public GameObject pause;
    public TimeManager tm;
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
                Time.timeScale = 0;
            }
            else if(menuOpen && pause.activeSelf)
            {
                
                pause.SetActive(false);
                ResumeTime();
            }
            
        }
    }

    public void ResumeTime()
    {
        menuOpen = false;
        Time.timeScale = tm.resume;
    }

    public void GoToScene(int num)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(num);
    }
}
