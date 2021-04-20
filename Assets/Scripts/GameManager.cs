using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine;
//nothing's here at the moment, obviously-I feel like we can probably put most of the gameObject scripts in here for ease of management?
public class GameManager : MonoBehaviour
{
    public GameObject[] toEnable;

    public GameObject[] toDisable;

    private void Awake()
    {
        foreach (var item in toEnable)
        {
            item.SetActive(true);
        }

        foreach (var item in toDisable)
        {
            item.SetActive(false);
        }
    }

}
