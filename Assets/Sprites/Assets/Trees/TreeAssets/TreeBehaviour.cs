using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeBehaviour : MonoBehaviour
{

    private Animator anim;

    // Start is called before the first frame update
    private void Start()
    {
        anim = GetComponent<Animator>();
        anim.speed = Random.Range(1, 5);
    }

    // Update is called once per frame
    void Update()
    {
        if(anim.speed > .6f)
        {
            anim.speed -= .01f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        anim.speed = 5;
    }
}
