using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPowerPlantScript : MonoBehaviour
{
    public bool providingPower = true;

  

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
