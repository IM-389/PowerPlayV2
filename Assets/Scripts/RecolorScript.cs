using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecolorScript : MonoBehaviour
{
    public GameObject[] colorableObjects;

    public void Recolor(Color color)
    {
        foreach (GameObject obj in colorableObjects)
        {
            obj.GetComponent<SpriteRenderer>().color = color;
        }
    }
}
