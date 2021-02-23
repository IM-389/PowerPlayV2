using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineCircleScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(-2f, -2f, .5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (BuildFunctions.position1 != new Vector2(0, 0) && BuildFunctions.menuSelection == 1)
        {
            transform.position = new Vector3(BuildFunctions.position1.x, BuildFunctions.position1.y,.5f);
        }
        else if(BuildFunctions.menuSelection == 0)
        {
            transform.position = new Vector3(Helper.getMousePositionFromWorld().x, Helper.getMousePositionFromWorld().y, .5f);
            Helper.SnapToGrid(transform);
        }
        else
        {
            transform.position = new Vector3(-2f, -2f, .5f);
        }
    }
}
