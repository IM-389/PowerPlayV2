using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

public class BuildMenuFunctions : MonoBehaviour
{
    //toggle if place object tool is in use
    //toggled in DropDownMenuHandler function
    public bool toolInUse = true;

    //bool used to know when tool is in use
    public bool lineRunner = false;

    //bool used to know when tool is in use
    public bool removerToolBool = false;

    //text that appears on bottom of the screen to tell player how to stop placeing object/using tool
    public GameObject toolPromptObject;

    //text component of toolprompt object
    public Text toolPromptText;

    //dropdown ui
    public TMPro.TMP_Dropdown dropDown;

    //stored gameobjects that can be placed
    //power lines
    [SerializeField]public GameObject selection1;

    //solar panel
    [SerializeField] public GameObject selection2;

    public static int solarAmount = 0;

    //wind turbine
    [SerializeField] public GameObject selection3;

    public static int turbineAmount = 0;

    //coal plant
    [SerializeField] public GameObject selection4;

    public static int coalAmount = 0;

    //natural gas
    [SerializeField] public GameObject selection5;

    public static int gasAmount = 0;

    //tree object
    [SerializeField] public GameObject treeObject;

    //house object
    [SerializeField] public GameObject houseObject;

    //selected gameobject
    [SerializeField] public static GameObject selectedGameObject;

    //positions used to lay lines in run lines tool
    public static Vector2 position1 = new Vector2(0, 0);
    public static Vector2 position2 = new Vector2(0, 0);

    //this is a 2d grid used to store locations of objects on the game board
    //[0,0] in the array = (0,0) in the game world
    //objects names are added to this array via the setGridSpace function found below
    //you can check if a space in the grid is occupied via the isGridSpaceEmpty function found below
    //positions horizontal or vertical of [0,0] remain empty
    //this prevents errors when using the checkSurroundingGridSpaces function below
    public static GameObject[,] playArea = new GameObject[101, 101];

    //used to count up in setHomeToPower function
    public static int connectionNumber = 0;

    public static uint lineNumber = 0;
    public static Vector2[] lineLocations = new Vector2[200];
    public static GameObject[] lineObjects = new GameObject[200];

    public void Start()
    {
        //find and set the toolprompt object
        toolPromptObject = GameObject.Find("ToolPrompt");

        //find and set the toolprompt component
        toolPromptText = toolPromptObject.GetComponent<Text>();

        //find and set the drop down object
        dropDown = GameObject.Find("Dropdown").GetComponent<TMPro.TMP_Dropdown>();

        //set selected game object to the selected game object
        selectedGameObject = selection1;
        dropDown = GameObject.Find("Dropdown").GetComponent<TMPro.TMP_Dropdown>();     
        dropDown = GameObject.Find("Dropdown").GetComponent<TMPro.TMP_Dropdown>();     
    }

    public void Update()
    {
        //if an object has been selected then the tool is active
        //therefore the function will run
        if (toolInUse == true)
        {
            PlaceObjectTool();
            
        }

        if (lineRunner == true)
        {
            RunLinesTool();
        }

        if (removerToolBool == true)
        {
            RemoverTool();
        }

        //exit tool
if (Input.GetMouseButtonDown(1) && dropDown.value == 5 && position1 != new Vector2(0, 0))
        {
            position1 = new Vector2(0, 0);
            //set text on screen blank
            toolPromptText.text = "";
        }

        //Reset draw line circle
        if(dropDown.value != 5)
        {
            position1 = new Vector3(0, 0);

            //set text on screen blank
            toolPromptText.text = "";
        }
    }

    //determines what placeable object is selected on drop down menu
    //Genuine BIG BOY
    public void DropDownMenuHandler(int selection)
    {
       switch(selection)
        {
            case 0:
                //power line
                toolInUse = true;

                //run lines tool
                lineRunner = false;

                //set selected game object to the selected game object
                selectedGameObject = selection1;

                //set remove tool bool
                removerToolBool = false;
                break;
            case 1:
                //solar power
                toolInUse = true;

                //run lines tool
                lineRunner = false;

                //set selected game object to the selected game object
                selectedGameObject = selection2;

                //set remove tool bool
                removerToolBool = false;
                break;
            case 2:
                //wind turbine
                toolInUse = true;

                //run lines tool
                lineRunner = false;

                //set selected game object to the selected game object
                selectedGameObject = selection3;

                //set remove tool bool
                removerToolBool = false;
                break;
            case 3:
                //coal plant
                toolInUse = true;

                //run lines tool
                lineRunner = false;

                //set selected game object to the selected game object
                selectedGameObject = selection4;

                //set remove tool bool
                removerToolBool = false;
                break;
            case 4:
                //natural gas plant
                toolInUse = true;

                //run lines tool
                lineRunner = false;

                //set selected game object to the selected game object
                selectedGameObject = selection5;

                //set remove tool bool
                removerToolBool = false;
                break;
            case 5:
                //place lines tool
                toolInUse = false;

                //run lines tool
                lineRunner = true;

                //set selected game object to the selected game object
                selectedGameObject = null;

                //set remove tool bool
                removerToolBool = false;
                break;
            case 6:
                //remove object tool
                toolInUse = false;

                //run lines tool
                lineRunner = false;

                //set selected game object to the selected game object
                selectedGameObject = null;

                //set remove tool bool
                removerToolBool = true;
                break;
        }
    }

    //place object tool
    public void PlaceObjectTool()
    {
        //check for input and check if click is not on ui
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            //check to make sure grid is empty
            if (IsGridSpaceEmpty(Helper.getMousePositionFromWorldRounded()))
            {
                //create object on location
                

                if (selectedGameObject == selection2 && Phase2Manager.currency >= SolarScript.cost)
                {
                    solarAmount++;
                    Instantiate(selectedGameObject, Helper.getMousePositionFromWorld(), transform.rotation);
                }
                else if (selectedGameObject == selection3 && Phase2Manager.currency >= TurbineScript.cost)
                {
                    turbineAmount++;
                    Instantiate(selectedGameObject, Helper.getMousePositionFromWorld(), transform.rotation);
                }
                else if (selectedGameObject == selection4 && Phase2Manager.currency >= CoalScript.cost)
                {
                    coalAmount++;
                    Instantiate(selectedGameObject, Helper.getMousePositionFromWorld(), transform.rotation);
                }
                else if(selectedGameObject == selection5 && Phase2Manager.currency >= NaturalGasScript.cost)
                { 
                    gasAmount++;
                    Instantiate(selectedGameObject, Helper.getMousePositionFromWorld(), transform.rotation);
                }

                GameObject.Find("GameManager").GetComponent<Phase2Manager>().UpdateUi(coalAmount, turbineAmount, gasAmount, solarAmount);

                //determine how many and what extra spaces need to be filled
                AddGridSpaces(selection1, selection2, selection3, selection4, selection5);
            }
        }
    }

    //run lines tool
    public void RunLinesTool()
    {
        if (position1 != new Vector2(0, 0))
        {
            //enable on screen text
            toolPromptText.text = "Right Click To Stop Placing Cable";
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!IsGridSpaceEmpty(Helper.getMousePositionFromWorldRounded()) && position1 == new Vector2(0,0) && !IsGridSpaceMatching(treeObject, Helper.getMousePositionFromWorldRounded()))
            {
                position1 = Helper.getMousePositionFromWorldRounded();

            }
            else if (!IsGridSpaceEmpty(Helper.getMousePositionFromWorldRounded()) && position1 != new Vector2(0, 0) && position1 != new Vector2(Helper.getMousePositionFromWorldRounded().x,Helper.getMousePositionFromWorldRounded().y) && !IsGridSpaceMatching(treeObject, Helper.getMousePositionFromWorldRounded()))
            {
                if (Helper.getDistanceFromPosition(new Vector3(position1.x,position1.y,0), new Vector3(Helper.getMousePositionFromWorldRounded().x, Helper.getMousePositionFromWorldRounded().y,0)) <= 3.5)
                {
                    position2 = Helper.getMousePositionFromWorldRounded();

                    if (IsGridSpaceMatching(houseObject, new Vector3(position1.x, position1.y, 0)) && IsGridSpaceMatching(houseObject, new Vector3(position2.x, position2.y, 0)))
                    {
                        position1 = new Vector2(0, 0);
                        position2 = new Vector2(0, 0);
                    }
                    else
                    {
                        Helper.DrawLine(position1, position2, Color.white);

                        position1 = new Vector2(0, 0);
                        position2 = new Vector2(0, 0);
                    }
                }
            }
        }

    }

    //tool that remove things
    public void RemoverTool()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            if (!IsGridSpaceEmpty(Helper.getMousePositionFromWorldRounded()))
            {
                //remove grid spaces
                RemoveExtraGridSpaces(Helper.getObjectFromClick(), selection1, selection2, selection3, selection4, selection5);
            }
        }
    }

    //determines if a position contains an instance of a specific game object
    public static bool IsGridSpaceMatching(GameObject objectToCompare, Vector3 location)
    {
        return (objectToCompare == playArea[(int)location.x, (int)location.y]);
    }

    //checks array to see if space is null
    public static bool IsGridSpaceEmpty(Vector3 locationToCheck)
    {
        if (playArea[(int)locationToCheck.x, (int)locationToCheck.y] == null)
        {
            //return true if object exists in location
            return true;
        }
        else
        {
            //return false if object does not exist in location
            return false;
        }

        /*

        if (selectedGameObject == selection1)
        {
            //fill grid space with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded());         
        }
        else if (selectedGameObject == selection2)
        {
            //fill grid space with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded());
        }
        else if (selectedGameObject == selection3)
        {
            //turbine
            //fill grid space with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded());
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(0, 1, 0));
        }
        else if (selectedGameObject == selection4)
        {
            //fill grid space with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded());
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(0, 1, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-1, 1, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-1, 0, 0));
            Phase2Manager.currency += 10;
        }
        else if (selectedGameObject == selection5)
        {
            //fill grid space with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded());
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(0, 1, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(0, 2, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-1, 2, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-1, 1, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-1, 0, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-2, 2, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-2, 1, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-2, 0, 0));
        }
        */
    }

    //set a grid space
    public static void SetGridSpace(GameObject entry, Vector3 location)
    {
        playArea[(int)location.x, (int)location.y] = entry;
    }

    //check surrounding grid spaces for specific object
    public static bool CheckSurroundingGridSpaces(Vector3 PositionToCheck, GameObject objectToCheckFor)
    {
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (!(x == 0 && y == 0))
                {
                    if (IsGridSpaceMatching(objectToCheckFor, new Vector3(PositionToCheck.x + x, PositionToCheck.y + y, 0)))
                    {
                        return false;
                    }
                }
            }
        }
        return true;
    }

    //empty a grid space
    public static void ClearGridSpace(Vector3 locationToClear)
    {
        playArea[(int)locationToClear.x, (int)locationToClear.y] = null;
    }

    //add objects spaces to grid
    public static void AddGridSpaces(GameObject selection1, GameObject selection2, GameObject selection3, GameObject selection4, GameObject selection5)
    {
        if (selectedGameObject == selection1)
        {
            //fill grid space with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded());
        }
        else if (selectedGameObject == selection2)
        {
            //fill grid space with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded());
            Phase2Manager.currency -= 20;
        }
        else if (selectedGameObject == selection3)
        {
            //turbine
            //fill grid space with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded());
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(0, 1, 0));

            Phase2Manager.currency -= 15;
        }
        else if (selectedGameObject == selection4)
        {
            //fill grid space with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded());
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(0, 1, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-1, 1, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-1, 0, 0));
            Phase2Manager.currency -= 10;
        }
        else if (selectedGameObject == selection5)
        {
            //fill grid space with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded());
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(0, 1, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(0, 2, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-1, 2, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-1, 1, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-1, 0, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-2, 2, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-2, 1, 0));
            //fill grid space above with object data
            SetGridSpace(selectedGameObject, Helper.getMousePositionFromWorldRounded() + new Vector3(-2, 0, 0));
            Phase2Manager.currency -= 20;
        }
    }

    public static void RemoveExtraGridSpaces(GameObject objectToBeRemoved, GameObject selection1, GameObject selection2, GameObject selection3, GameObject selection4, GameObject selection5)
    {
        if (objectToBeRemoved.name == selection1.name + "(Clone)")
        {
            //power pole
            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0));
        }
        else if (objectToBeRemoved.name == selection2.name + "(Clone)")
        {
            //solar panel
            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0));
            Phase2Manager.currency -= 10;
        }
        else if (objectToBeRemoved.name == selection3.name + "(Clone)")
        {
            //turbine
            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y,0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0));
            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(0, 1, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(0, 1, 0));
            Phase2Manager.currency -= 8;
        }
        else if (objectToBeRemoved.name == selection4.name + "(Clone)")
        {
            //coal
            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(0, 1, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(0, 1, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-1, 1, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-1, 1, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-1, 0, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-1, 0, 0));
            Phase2Manager.currency -= 5;
        }
        else if (objectToBeRemoved.name == selection5.name + "(Clone)")
        {
            //gas
            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(0, 1, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(0, 1, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(0, 2, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(0, 2, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-1, 1, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-1, 1, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-1, 0, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-1, 0, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-1, 2, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-1, 2, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-2, 1, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-2, 1, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-2, 0, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-2, 0, 0));

            ClearGridSpace(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-2, 2, 0));
            RemoveLines(new Vector3(objectToBeRemoved.transform.position.x, objectToBeRemoved.transform.position.y, 0) + new Vector3(-2, 2, 0));

            Phase2Manager.currency += 10;
        }
    }

    public static void RemoveLines(Vector3 pointToCheck)
    {
        for(int count = 0 ; count <= lineNumber ; count++)
        {
            if (new Vector3(pointToCheck.x,pointToCheck.y,0) == new Vector3(lineLocations[count].x,lineLocations[count].y,0))
            {
                Object.DestroyImmediate(lineObjects[count]);
            }
        }
    }
}