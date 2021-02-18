using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSpawner : MonoBehaviour
{

    public GameObject idleGrid;
    Animator  gridAnimator;

    //This Script enables the actual grid after the animation of the grid opening up is complete.
    //It also pauses the animation of the grid opening to prevent looping that animation.

    private void Start()
    {
        gridAnimator = gameObject.GetComponent<Animator>();
    }

    //Called in the animator of the GridOpenAnim, stopping its animation and activating the idle grid.
    void GridOn()
    {

        idleGrid.SetActive(true);
        gridAnimator.speed = .0f;

    }


    //ResetGridSpeed is only here to be called at the start of the animation in the animator.
    //It resets the animation speed to 1, so that it will play after it has been set to 0 in the past.
    void ResetGridSpeed()
    {
        gridAnimator.speed = 1;
    }


}
