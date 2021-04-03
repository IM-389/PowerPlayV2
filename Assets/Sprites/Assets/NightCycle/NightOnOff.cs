using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightOnOff : MonoBehaviour
{
    public static bool night = false;

    void NightOn()
    {
        night = true;
        print("night");
    }

    void NightOff()
    {
        night = false;
        print("day");
    }
}
