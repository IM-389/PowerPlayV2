using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseLights : MonoBehaviour
{

    //This script just shows the house when it is powered or unpowered by changing the sprite and particles
    //when the player clicks. in the future this will be tied to actually being powered or not.

    bool powered = false;
    //public GameObject windows;
    private SpriteRenderer spriteR;
    public Sprite poweredSprite;
    public Sprite unpoweredSprite;
    public GameObject smoke;
    // Start is called before the first frame update


    private void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        CheckPower();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            powered =! powered;
            CheckPower();
        }
    }

    void CheckPower()
    {
        if(powered == false)
        {
            spriteR.sprite = unpoweredSprite;
            smoke.SetActive(false);
        }
        if(powered == true)
        {
            spriteR.sprite = poweredSprite;
            smoke.SetActive(true);
        }
    }
}
