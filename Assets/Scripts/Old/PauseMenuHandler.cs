using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuHandler : MonoBehaviour
{
    public GameObject pauseMenuCurtain;
    public GameObject helpScreen1;
    public GameObject nextButton1;
    public GameObject helpScreen2;
    public GameObject nextButton2;
    // Start is called before the first frame update
    public void HamburgerMenuButton()
    {
        SoundManager.PlaySound("menu");
        pauseMenuCurtain.SetActive(!pauseMenuCurtain.activeSelf);
    }

    public void ResumeGame()
    {
        SoundManager.PlaySound("menu");
        pauseMenuCurtain.SetActive(false);
    }

    public void HelpMenu()
    {
        SoundManager.PlaySound("menu");
        HelpScreenOne();
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

    public void ExitGame()
    {
        SoundManager.PlaySound("menu");
        Application.Quit();
    }

}
