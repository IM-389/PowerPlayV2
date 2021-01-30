using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCreateLineTool : MonoBehaviour
{

    [SerializeField]public GameObject powerLine;
    public bool toolInUse = false;
    [SerializeField] public static Vector2 mousePosition1;
    [SerializeField] public static Vector2 mousePosition2;
    [SerializeField] public static Vector2 centerPosition;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(0) && toolInUse == true)
        {
            mousePosition2 = Helper.getMousePositionFromWorld();
            Debug.Log("mousePosition2" + mousePosition2);
            Vector2 centerPosition = new Vector2(mousePosition1.x + mousePosition2.x, mousePosition1.y + mousePosition2.y) / 2f;
            Debug.Log(centerPosition);
            Instantiate(powerLine);

            toolInUse = false;
        }

        if (Input.GetMouseButtonDown(0) && toolInUse == false)
        {
            mousePosition1 = Helper.getMousePositionFromWorld();
            Debug.Log("mousePosition1" + mousePosition1);
            toolInUse = true;
        }
    }




    //obtain location a first click
    //obtain location b second click or release of click
    //create object that spans between first and second position


}
