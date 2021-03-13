using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CornerObjectUI : MonoBehaviour
{

    //placeable objects to get sprites from
    [SerializeField] public GameObject cornerObject;
    [SerializeField] public GameObject poleTitle;
    [SerializeField] public GameObject wiresTitle;
    [SerializeField] public GameObject solarTitle;
    [SerializeField] public GameObject windTitle;
    [SerializeField] public GameObject coalTitle;
    [SerializeField] public GameObject gasTitle;
    [SerializeField] public GameObject removeTitle;
    [SerializeField] public GameObject repairTitle;

    public GameObject SimulationUpdateObject;
    public GameObject EventPanelObject;

    private void Start()
    {
        cornerObject.SetActive(false);
        poleTitle.SetActive(false);
        wiresTitle.SetActive(false);
        solarTitle.SetActive(false);
        windTitle.SetActive(false);
        coalTitle.SetActive(false);
        gasTitle.SetActive(false);
        removeTitle.SetActive(false);
        repairTitle.SetActive(false);
    }

    private void Update()
    {
        cornerObject.SetActive(false);
        poleTitle.SetActive(false);
        wiresTitle.SetActive(false);
        solarTitle.SetActive(false);
        windTitle.SetActive(false);
        coalTitle.SetActive(false);
        gasTitle.SetActive(false);
        removeTitle.SetActive(false);
        repairTitle.SetActive(false);

        if (Input.mousePosition.x < Screen.width * .65 && !SimulationUpdateObject.activeInHierarchy && !EventPanelObject.activeInHierarchy)
        {
            cornerObject.SetActive(true);
            switch (BuildFunctions.menuSelection)
            {

                case 0:
                    poleTitle.SetActive(true);
                    break;
                case 1:
                    wiresTitle.SetActive(true);
                    break;
                case 2:
                    solarTitle.SetActive(true);
                    break;
                case 3:
                    windTitle.SetActive(true);
                    break;
                case 4:
                    coalTitle.SetActive(true);
                    break;
                case 5:
                    gasTitle.SetActive(true);
                    break;
                case 6:
                    removeTitle.SetActive(true);
                    break;
                case 7:
                    repairTitle.SetActive(true);
                    break;
            }
        }
        else
        {
            cornerObject.SetActive(false);
        }
    }
}
