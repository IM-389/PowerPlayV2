using Power.V2;
using UnityEngine;

public class HouseLights : MonoBehaviour
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

    public ConsumerScript consumer;

    private TimeManager timeManager;
    // Start is called before the first frame update

    private NetworkScript network;

    private void Start()
    {
        spriteR = gameObject.GetComponent<SpriteRenderer>();
        network = transform.parent.GetComponent<NetworkScript>();
        CheckPower();
    }

    private void Update()
    {
        powered = network.manager.hasEnoughPower;
        //powered = (storage.powerStored > consumer.consumptionCurve[timeManager.hours]);
        CheckPower();
    }

    void CheckPower()
    {
        if(powered == false)
        {
            spriteR.sprite = unpoweredSprite;
            smoke.SetActive(false);
            houseLight.SetActive(false);
        }
        if(powered == true)
        {
            spriteR.sprite = poweredSprite;
            houseLight.SetActive(true);
            smoke.SetActive(true);
        }
    }
}
