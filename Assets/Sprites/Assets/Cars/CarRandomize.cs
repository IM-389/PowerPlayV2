using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRandomize : MonoBehaviour
{
    SpriteRenderer carSpriteR;
    Sprite randomizedSprite;

    public Sprite carSpriteOne;
    public Sprite carSpriteTwo;
    public Sprite carSpriteThree;
    public Sprite carSpriteFour;
    public GameObject parent;

    int randN;
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.transform.localScale = new Vector3(5, 5, 1);

       carSpriteR = gameObject.GetComponent<SpriteRenderer>();

        randN = Random.Range(1, 5);


        if(randN == 1)
        {
            randomizedSprite = carSpriteOne;
        }
        else if(randN == 2)
        {
            randomizedSprite = carSpriteTwo;
        }
        else if (randN == 3)
        {
            randomizedSprite = carSpriteThree;
        }
        else if (randN == 4)
        {
            randomizedSprite = carSpriteFour;
        }

        carSpriteR.sprite = randomizedSprite;
    }

    private void Update()
    {
        //transform.rotation.Translate()
        //transform.rotation = Vector3.RotateTowards(gameObject.transform.rotation, transform.forward, 1, 0.0f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "despawner")
        {
            Destroy(parent);
        }
    }

}
