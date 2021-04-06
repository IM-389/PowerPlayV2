using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopBarScript : MonoBehaviour
{
    public GameObject powerPanel;
    public GameObject populationPanel;

    public void PowerButtonFunction()
    {
        powerPanel.SetActive(!powerPanel.activeSelf);
    }

    public void PopulationButtonFunction()
    {
        populationPanel.SetActive(!populationPanel.activeSelf);
    }
}
