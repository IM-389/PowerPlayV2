using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScript : MonoBehaviour
{
    //object sprite render
    SpriteRenderer sprRend;

    //Color component of sprite renderer
    Color sprRendCol;

    //Transparency amount
    public float transLevel = .45f;


    //placeable objects to get sprites from
    [SerializeField] public GameObject electricPole;
    [SerializeField] public GameObject solarPanel;
    [SerializeField] public GameObject windTurbine;
    [SerializeField] public GameObject coalPlant;
    [SerializeField] public GameObject gasPlant;


    // Start is called before the first frame update
    void Start()
    {
        sprRend = GetComponent<SpriteRenderer>();
        sprRendCol = sprRend.color;
        sprRendCol.a = transLevel;
    }

    // Update is called once per frame
    void Update()
    {
        //check if mouse is on screen
        if (Helper.IsMouseOnScreen())
        {
            transform.position = new Vector3(Helper.getMousePositionFromWorldRounded().x, Helper.getMousePositionFromWorldRounded().y, 0);

            if (BuildFunctions.areGridSpacesEmpty(Helper.getMousePositionFromWorldRounded()))
            {
                sprRend.color = new Color(1, 1, 1, transLevel);
            }
            else
            {
                sprRend.color = new Color(1, 0, 0, transLevel);
            }


            switch (BuildFunctions.menuSelection)
            {
                case (0):
                    sprRend.sprite = electricPole.GetComponent<SpriteRenderer>().sprite;
                    transform.localScale = new Vector3(.1f, .1f, 1);
                    break;
                case (2):
                    sprRend.sprite = solarPanel.GetComponent<SpriteRenderer>().sprite;
                    transform.localScale = new Vector3(.1f, .1f, .1f);
                    break;
                case (3):
                    sprRend.sprite = windTurbine.GetComponent<SpriteRenderer>().sprite;
                    transform.localScale = new Vector3(.3f, .3f, .1f);
                    break;
                case (4):
                    sprRend.sprite = coalPlant.GetComponent<SpriteRenderer>().sprite;
                    transform.localScale = new Vector3(.3f, .3f, .1f);
                    break;
                case (5):
                    sprRend.sprite = gasPlant.GetComponent<SpriteRenderer>().sprite;
                    transform.localScale = new Vector3(.2f, .2f, .1f);
                    break;
                default:
                    sprRend.sprite = null;
                    break;


            }

        }
    }
}
