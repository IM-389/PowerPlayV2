using Power.V2;
using UnityEngine;

public class PoweredVisual : MonoBehaviour
{

    //This script just shows the house when it is powered or unpowered by changing the sprite and particles
    //when the player clicks. in the future this will be tied to actually being powered or not.

    bool powered = false;
    //public GameObject windows;
    private SpriteRenderer spriteR;
    public Sprite poweredSprite;
    public Sprite unpoweredSprite;
    public GameObject smoke;
    public GameObject houseLight;
    public bool usesSprite;

    public ConsumerScript consumer;

    public Animator anim;

    private TimeManager timeManager;
    // Start is called before the first frame update

    private NetworkScript network;

    private void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        network = transform.GetComponent<NetworkScript>();
        CheckPower();
        //anim = gameObject.GetComponent<Animator>();
    }

    private void Update()
    {
        powered = network.manager.hasEnoughPower;
        //powered = (storage.powerStored > consumer.consumptionCurve[timeManager.hours]);
        CheckPower();
    }

    void CheckPower()
    {
        if (powered == false)
        {
            if (usesSprite == true)
            {
                spriteR.sprite = unpoweredSprite;
            }

            smoke.SetActive(false);
            houseLight.SetActive(false);
            anim.speed = 0;
            print("LETSGO");
        }
        if (powered == true)
        {
            if (usesSprite == true)
            {
                spriteR.sprite = poweredSprite;
            }
            houseLight.SetActive(true);
            smoke.SetActive(true);
            anim.speed = 1;
            print("YYYYY");

        }
    }
}