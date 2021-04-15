using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedRandomizer : MonoBehaviour
{
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(.4f, 1.1f);
    }

}
