using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolIconUI : MonoBehaviour
{
    //placeable objects to get sprites from
    [SerializeField] public Sprite lineToolIcon;
    [SerializeField] public Sprite removeToolIcon;
    [SerializeField] public Sprite electricPoleIcon;
    [SerializeField] public Sprite solarPanelIcon;
    [SerializeField] public Sprite windTurbineIcon;
    [SerializeField] public Sprite coalPlantIcon;
    [SerializeField] public Sprite gasPlantIcon;
    [SerializeField] public Sprite repairToolIcon;

    Image iconImage;

    private void Start()
    {
        iconImage = GetComponent<Image>();
    }

    private void Update()
    {
        switch (BuildFunctions.menuSelection)
        {
            case 0:
                iconImage.sprite = electricPoleIcon;
                break;
            case 1:
                iconImage.sprite = lineToolIcon;               
                break;
            case 2:
                iconImage.sprite = solarPanelIcon;
                break;
            case 3:
                iconImage.sprite = windTurbineIcon;               
                break;
            case 4:
                iconImage.sprite = coalPlantIcon;              
                break;
            case 5:
                iconImage.sprite = gasPlantIcon;
                break;
            case 6:
                iconImage.sprite = removeToolIcon;
                break;
            case 7:
                iconImage.sprite = repairToolIcon;
                break;
            default:
                iconImage.sprite = null;
                break;
        }
    }
}
