using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Power.V2;

public class CityApprovalTracker : MonoBehaviour
{
    public GameObject citySat;
    void Start()
    {
        GameObject generator = GameObject.FindGameObjectWithTag("Generator");
        GameObject consumer = GameObject.FindGameObjectWithTag("house");
        consumer.GetComponent<ConsumerScript>().TrackApproval();
        citySat.transform.GetChild(0).gameObject.SetActive(true);
    }

}
