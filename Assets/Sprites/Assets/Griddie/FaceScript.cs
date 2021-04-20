using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceScript : MonoBehaviour
{
    public Animator anim;
    public SpriteRenderer spr;
    public GameObject griddieFace;

    public static bool switchFaces = false;
    public static int faceSprite = 1;

    public Sprite faceOne;
    public Sprite faceTwo;
    public Sprite faceThree;
    public Sprite faceFour;
    public Sprite faceFive;
    public Sprite faceSix;
    public Sprite faceSeven;
    public Sprite faceEight;



    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        spr = griddieFace.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(switchFaces == true)
        {
            SwitchFace();
            //anim.Play("FaceSwap");
            switchFaces = false;
        }
    }

    void SwitchFace()
    {
        if(faceSprite == 1)
        {
            spr.sprite = faceOne;
        }
        else if(faceSprite == 2)
        {
            spr.sprite = faceTwo;
        }
        else if (faceSprite == 3)
        {
            spr.sprite = faceThree;
        }
        else if (faceSprite == 4)
        {
            spr.sprite = faceFour;
        }
        else if (faceSprite == 5)
        {
            spr.sprite = faceFive;
        }
        else if (faceSprite == 6)
        {
            spr.sprite = faceSix;
        }
        else if (faceSprite == 7)
        {
            spr.sprite = faceSeven;
        }
        else if (faceSprite == 8)
        {
            spr.sprite = faceEight;
        }
    }
}
