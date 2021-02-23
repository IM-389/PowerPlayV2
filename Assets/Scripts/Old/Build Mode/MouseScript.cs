using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseScript : MonoBehaviour
{
    //object sprite render
    SpriteRenderer sprRend;

    //Color component of sprite renderer
    Color sprRendCol;

    //Transparency amount
    public float transLevel = .45f;

    public List<RaycastHit2D> hitPoints = new List<RaycastHit2D>();

    public BuildScript buildScript;
    private Camera mainCamera;


    // Start is called before the first frame update
    void Start()
    {
        sprRend = GetComponent<SpriteRenderer>();
        sprRendCol = sprRend.color;
        sprRendCol.a = transLevel;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // If the mouse is over UI, ignore this function
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
        // Get the mouses world position
        Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPosRounded = RoundVector(mouseWorldPos);
        transform.position = mouseWorldPosRounded;
        bool blocked = false;
        PlaceableScript placeable = buildScript.selectedBuilding.GetComponent<PlaceableScript>();

        for (int i = 0; i > -placeable.dimensions.x; i--)
        {
            for (int j = 0; j < placeable.dimensions.y; j++)
            {
                RaycastHit2D hitPoint = Physics2D.Raycast(mouseWorldPosRounded + new Vector2(i, j), Vector2.zero);
                hitPoints.Add(hitPoint);
            }
        }
        foreach (RaycastHit2D hitPoint in hitPoints)
        {
            if (!hitPoint.transform.CompareTag("Background"))
            {
                blocked = true;
            }
        }
        if (!blocked)
        {
            sprRend.color = new Color(1, 1, 1, transLevel);
        }
        else
        {
            sprRend.color = new Color(1, 0, 0, transLevel);
        }
        hitPoints.Clear();
        sprRend.sprite = buildScript.selectedBuilding.GetComponent<SpriteRenderer>().sprite;
        transform.localScale = buildScript.selectedBuilding.transform.localScale;
        /*
        //check if mouse is on screen
        if (Helper.IsMouseOnScreen())
        {
            // TODO: Cache mouse position
            transform.position = new Vector3(Helper.getMousePositionFromWorldRounded().x, Helper.getMousePositionFromWorldRounded().y, 0);

            if (BuildFunctions.areGridSpacesEmpty(Helper.getMousePositionFromWorldRounded()))
            {
                sprRend.color = new Color(1, 1, 1, transLevel);
            }
            else
            {
                sprRend.color = new Color(1, 0, 0, transLevel);
            }

            // TODO: Standardize and generalize
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
        */
    }
    private Vector2 RoundVector(Vector2 vec)
    {
        vec.x = (float)Math.Round(vec.x);
        vec.y = (float)Math.Round(vec.y);
        return vec;
    }
}
