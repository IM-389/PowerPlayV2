﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gasBrokenIconScript : MonoBehaviour
{

    NaturalGasScript parentScript;
    SpriteRenderer sprRend;
    public Sprite brokenSprite;
    public Sprite noPowerSprite;

    bool toggleDirection = false;



    // Start is called before the first frame update
    void Start()
    {
        parentScript = transform.parent.GetComponent<NaturalGasScript>();
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


        if (parentScript.broken == true)//if im not mistaken, this should change sprites to their "broken" form. 
        {//hoever, i dont think we need all this. should be able to just swap the sprites as long as the position stays the same, right? 
            //this seems too complex for that. 
            
            sprRend.sprite = brokenSprite;
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
        else if (parentScript.daysToBeBroken > 0 && !parentScript.broken)
        {
            sprRend.sprite = noPowerSprite;
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
