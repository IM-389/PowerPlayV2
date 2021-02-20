﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SolarScriptBrokenIcon : MonoBehaviour
{

    SolarScript parentScript;
    SpriteRenderer sprRend;
    public Sprite brokenSprite;
    public Sprite noPowerSprite;
    public Sprite noSunSprite;
    bool toggleDirection = false;



    // Start is called before the first frame update
    void Start()
    {
        parentScript = transform.parent.GetComponent<SolarScript>();
        sprRend = gameObject.GetComponent<SpriteRenderer>();
        sprRend.sprite = null;
    }

    // Update is called once per frame
    void FixedUpdate()
    {


        /*
    //changes object color to represent state
    if (connectedToPower)
    {
        if (neededPower == MaxneededPower)
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 1, 0, 1);
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().color = new Color(0, 0, 1, 1);
        }
    }
    else
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 0, 0, 1);
    }
    */

if (parentScript.daysToBeBroken > 0 )//i dont think we need all this, we should be able to just change the sprites and call it a day as long as we have the proper position, right? same problem with
            //all the other broken icon scripts
        {
            sprRend.sprite = noSunSprite;
            if (toggleDirection == true)
            {
                transform.localPosition += new Vector3(0, .01f, 0);
                if (transform.localPosition.y >= 8.5f)
                {
                    toggleDirection = false;
                }
            }
            else
            {
                transform.localPosition += new Vector3(0, -.01f, 0);
                if (transform.localPosition.y <= 7)
                {
                    toggleDirection = true;
                }

            }
        }
        else
        {
            sprRend.sprite = null;

        }

        /*
        else
        {

            sprRend.sprite = noConnectionSprite;
            if (toggleDirection == true)
            {
                transform.localPosition += new Vector3(0, .01f, 0);
                if (transform.localPosition.y >= 8.5f)
                {
                    toggleDirection = false;
                }
            }
            else
            {
                transform.localPosition += new Vector3(0, -.01f, 0);
                if (transform.localPosition.y <= 7)
                {
                    toggleDirection = true;
                }

            }
        }
        */
    }
}