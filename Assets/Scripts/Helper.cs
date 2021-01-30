using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Helper
{
    //Snap object to grid
    public static void SnapToGrid(Transform transform)
    {
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.y)*.1f);
    }

    //gets the mouse position from the world and returns it rounded
    public static Vector3 getMousePositionFromWorldRounded()
    {
        Vector3 position = Input.mousePosition;
        position = Camera.main.ScreenToWorldPoint(position);
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        return position;
    }

    //gets distance between two points
    public static float getDistanceFromPosition(Vector3 object1,Vector3 object2)
    {
       return Vector3.Distance(object1, object2);
    }

    //gets the in world mouse position
    public static Vector3 getMousePositionFromWorld()
    {
        Vector3 position = Input.mousePosition;
        position = Camera.main.ScreenToWorldPoint(position);
        return position;
    }

    //returns screen pixel location for either game window or screen
    //im not sure which and im too tired to check
    public static Vector3 getMousePositionFromScreen()
    {
        return Input.mousePosition;
    }

    //Draws a line
    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        GameObject myLine = new GameObject();
        myLine.name = "powerLine";
        myLine.transform.position = start;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = .02f;
        lr.endWidth = .02f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        //GameObject.Destroy(myLine, duration);//useful but not what we are going for
        BuildFunctions.lineObjects[BuildFunctions.lineNumber] = myLine;
        BuildFunctions.lineLocations[BuildFunctions.lineNumber] = start;
        BuildFunctions.lineNumber += 1;
        BuildFunctions.lineObjects[BuildFunctions.lineNumber] = myLine;
        BuildFunctions.lineLocations[BuildFunctions.lineNumber] = end;
        BuildFunctions.lineNumber ++;

    }

    //get object 2d via raycast 2d 
    //might require 2d collider on object
    public static GameObject getObjectFromClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null)
            {
                return hit.collider.gameObject;

            }
            return null;
        }
        return null;
    }

    //get object from location 2d
    public static GameObject GetObjectFromLocation2d(Vector2 locationToCheck)
    {
        RaycastHit2D hit = Physics2D.Raycast(locationToCheck, Vector2.zero);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        return null;
    }

    //returns true if mouse is onscreen
    public static bool IsMouseOnScreen()
    {
        Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);
        return (screenRect.Contains(Input.mousePosition));
    }

    public static GameObject getObjectFromMousePosition()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;

        }
        return null;
    }
}
