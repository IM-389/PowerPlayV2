using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTransformerScript : MonoBehaviour
{
    public bool suppliedPower = false;
    public bool providingPower = false;

    // Start is called before the first frame update
    void Start()
    {
        Helper.SnapToGrid(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
