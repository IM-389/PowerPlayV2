using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPref;
    float randomDelay;
    // Start is called before the first frame update
    void Start()
    {
        randomDelay = 5;
        Invoke("spawnCar", randomDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void spawnCar()
    {
        Instantiate(carPref);
        randomDelay = Random.Range(4, 20);
        Invoke("spawnCar", randomDelay);
    }
}
