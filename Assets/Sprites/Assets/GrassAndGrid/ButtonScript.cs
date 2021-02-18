using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    public GameObject grid;

    Animator gridOpeningAnimator;

    //This is for the button, in order to set the grid on or off.

    private void Start()
    {
        gridOpeningAnimator = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        if (gameObject == true)
        {
            
        }
    }

    void GridOn()
    {
        grid.SetActive(true);
    }

    void GridOff()
    {
        grid.SetActive(false);
    }

}
