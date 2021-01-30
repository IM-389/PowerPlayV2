using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StartUpScript : MonoBehaviour
{
    //obtain the camera being used
    [SerializeField] public Camera mainCamera;

    //rate of gameobject spawns
    public float startingNumberOfHouses = 1;
    public float startingNumberOfTrees = 1.5f;

    public static int houseAmount = 0;

    //rate of spawn for startup
    private uint objectSpawnRate;
    private uint treeSpawnRate;

    //starting height and width of the array
    private float startingHeight;
    private float startingWidth;

    private float currentWidth;
    private float currentHeight;

    //value used to offset spawning objects from side of grid
    public int offsetDistance = 1;

    //gameobjects to be created on startup
    [SerializeField] public GameObject house;
    [SerializeField] public GameObject tree;

    public int maxHeight = 28;
    public int maxWidth = 48;

    //variables used to determine where to place random objects
    private int xPos = 0;
    private int yPos = 0;

    // Start is called before the first frame update
    void Start()
    {
        //makes startup one step later
        Invoke("briefPause", .00001f);
    }

    //starts 1 frame after start this gives the camera time to run cameraScript
    void briefPause()
    {
        //determine the starting amount of gridspaces
        startingWidth = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, 0)).x - .5f;
        startingHeight = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, 0)).y - .5f;

        //round down
        startingHeight = Mathf.Floor(startingHeight);
        startingWidth = Mathf.Floor(startingWidth);

        //calculate the spawn rate using height width and a good number 16 is thick, 24 is medium, 30 is bare
        objectSpawnRate = (uint)((startingHeight * startingWidth) / 30);
        treeSpawnRate = (uint)((maxHeight * maxWidth) / 30);


        //determine the amount of trees and houses to create
        startingNumberOfHouses = GameObject.Find("GameManager").GetComponent<Phase2Manager>().population;
        startingNumberOfTrees = startingNumberOfTrees * treeSpawnRate;

        //spawn houses
        spawnHouses();

        //spawn trees
        spawnTrees();
    }

    //spawn houses
    void spawnHouses()
    {
        //spawn houses
        while (startingNumberOfHouses > 0)
        {
            // Makes things more random APPARENTLY
            Random.InitState((int)System.DateTime.Now.Ticks);

            //assign x position a random number for spawn
            xPos = Random.Range(offsetDistance + 2, (int)startingWidth + 1);

            //assign y position a random number for spawn
            yPos = Random.Range(offsetDistance + 3, (int)startingHeight-1);

            //determine if generated position is empty
            if (BuildFunctions.IsGridSpaceEmpty(new Vector3(xPos, yPos, 0)))
            {
                //determine if position already has an instance next to it
                if (BuildFunctions.CheckSurroundingGridSpaces(new Vector3(xPos, yPos, 0), house))
                {
                    //create instance
                    Instantiate(house, new Vector3(xPos, yPos, 0), transform.rotation);

                    //set space in grid
                    BuildFunctions.SetGridSpace(house, new Vector3(xPos, yPos, 0));

                    //decrement counter
                    startingNumberOfHouses--;

                    houseAmount++;
                }
            }
        }
    }

    //spawn trees
    void spawnTrees()
    {
        //spawn trees
        while (startingNumberOfTrees > 0)
        {
            //assign x position a random number for spawn
            xPos = Random.Range(offsetDistance + 2, maxWidth);

            //assign y position a random number for spawn
            yPos = Random.Range(offsetDistance + 3, maxHeight);

            //determine if generated position is empty
            if (BuildFunctions.IsGridSpaceEmpty(new Vector3(xPos, yPos, 0)))
            {
                //determine if position already has an instance next to it
                if (BuildFunctions.CheckSurroundingGridSpaces(new Vector3(xPos, yPos, 0), tree))
                {
                    //create instance
                    Instantiate(tree, new Vector3(xPos, yPos, 0), transform.rotation);

                    //set space in grid
                    BuildFunctions.SetGridSpace(tree, new Vector3(xPos, yPos, 0));

                    //decrement counter
                    startingNumberOfTrees--;
                }

            }
        }
    }



    public void spawnMoreHouses(int housesToBeAdded)
    {
        int limiter = 50;

        while (housesToBeAdded > 0)
        {
            //determine the amount of usable gridspaces
            currentWidth = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, 0)).x - .5f;
            currentHeight = mainCamera.ScreenToWorldPoint(new Vector3(mainCamera.pixelWidth, mainCamera.pixelHeight, 0)).y - .5f;



            //assign x position a random number for spawn
            xPos = Random.Range(offsetDistance + 2, (int)currentWidth + 1);

            //assign y position a random number for spawn
            yPos = Random.Range(offsetDistance + 3, (int)currentHeight);

            //determine if generated position is empty
            if (BuildFunctions.IsGridSpaceEmpty(new Vector3(xPos, yPos, 0)))
            {
                //determine if position already has an instance next to it
                if (BuildFunctions.CheckSurroundingGridSpaces(new Vector3(xPos, yPos, 0), house))
                {
                    //create instance
                    Instantiate(house, new Vector3(xPos, yPos, 0), transform.rotation);

                    //set space in grid
                    BuildFunctions.SetGridSpace(house, new Vector3(xPos, yPos, 0));

                    houseAmount++;
                    housesToBeAdded--;
                }
            }

            limiter--;
            if(limiter <= 0)
            {
                break;
            }
        }

    }


    public void removeHouses(int housesToBeRemoved)
    {

        int limiter = 50;
        while (housesToBeRemoved > 0)
        {
            var objects = GameObject.FindGameObjectsWithTag("house");
            var objectCount = objects.Length;
            for(int i = 0;i< objectCount ;i++)
            {
                if (objects[i].GetComponent<HomeScript>().neededPower != objects[i].GetComponent<HomeScript>().MaxneededPower)
                {
                    //house
                    BuildFunctions.ClearGridSpace(new Vector3(objects[i].transform.position.x, objects[i].transform.position.y, 0));
                    BuildFunctions.RemoveLines(new Vector3(objects[i].transform.position.x, objects[i].transform.position.y, 0));
   
                    houseAmount--;
                    housesToBeRemoved--;
                }

                if(housesToBeRemoved <= 0)
                {
                    break;
                }
            }


            limiter--;
            if (limiter <= 0)
            {
                break;
            }

        }

        if(housesToBeRemoved > 0)
        {
            while (housesToBeRemoved > 0)
            {
                var objects = GameObject.FindGameObjectsWithTag("house");
                var objectCount = objects.Length;
                for (int i = 0; i < objectCount; i++)
                {

                        //house
                        BuildFunctions.ClearGridSpace(new Vector3(objects[i].transform.position.x, objects[i].transform.position.y, 0));
                        BuildFunctions.RemoveLines(new Vector3(objects[i].transform.position.x, objects[i].transform.position.y, 0));
                        houseAmount--;
                        housesToBeRemoved--;
                    

                    if (housesToBeRemoved <= 0)
                    {
                        break;
                    }
                }


                limiter--;
                if (limiter <= 0)
                {
                    break;
                }

            }
        }
        BuildFunctions.simulationReset = false;

    }
}
