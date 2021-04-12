using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightOnOff : MonoBehaviour
{
    public static bool night = false;

    public Animator nightAnimation;
    public void NightOn()
    {
        night = true;
        nightAnimation.SetBool("NightTimeStart", true);
        print("night");
    }

    public void NightOff()
    {
        night = false;
        nightAnimation.SetBool("NightTimeStart", false);
        print("day");
    }
}
