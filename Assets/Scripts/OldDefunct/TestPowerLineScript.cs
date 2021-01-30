using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPowerLineScript : MonoBehaviour
{
    public bool suppliedPower = false;
    public bool providingPower = false;
    public float scaleX;
    public float scaleY;

    // Start is called before the first frame update
    void Start()
    {
        print(("Scale x" + (TestCreateLineTool.mousePosition1.x - TestCreateLineTool.mousePosition2.x)));
        print(("Scale y" + (TestCreateLineTool.mousePosition1.y - TestCreateLineTool.mousePosition2.y)));
        //ScaleObject();
        transform.position = new Vector3(TestCreateLineTool.centerPosition.x, TestCreateLineTool.centerPosition.y,0);
        transform.localScale = new Vector3(scaleX, scaleY, 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

 

    void ScaleObject()
    {
        scaleX = Mathf.Abs(TestCreateLineTool.mousePosition1.x - TestCreateLineTool.mousePosition2.x);
        scaleY = Mathf.Abs(TestCreateLineTool.mousePosition1.y - TestCreateLineTool.mousePosition2.y);
    }





}
