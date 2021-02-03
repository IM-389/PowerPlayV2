using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{//logically, this all works, but I don't like it. I don't think we need a class to handle menu things, but I may be wrong there
    public GameObject helpScreen1;
    public GameObject nextButton1;
    public GameObject helpScreen2;
    public GameObject nextButton2;
    public void StartGameButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    public void HelpScreenOne()
    {
        helpScreen1.SetActive(true);
        nextButton1.SetActive(true);
    }

    public void HelpScreenTwo()
    {
        nextButton1.SetActive(false);
        helpScreen1.SetActive(false);
        nextButton2.SetActive(true);
        helpScreen2.SetActive(true);
    }

    public void CloseHelp()
    {
        nextButton2.SetActive(false);
        helpScreen2.SetActive(false);
    }

    public void ExitGameButton()
    {
        Application.Quit();
    }
}
