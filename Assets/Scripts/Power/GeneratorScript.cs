using System.Collections;
using UnityEngine;

public class GeneratorScript : PowerBase
{

    [Tooltip("What type of power the generator creates")]
    public PowerManager.POWER_TYPES type;

    [Tooltip("How much power is generated per unit time")]
    public float amount;
    private MoneyManager moneymanager;
    public int moneyGained;
    void Start()
    {
        moneymanager = GameObject.FindWithTag("GameController").GetComponent<MoneyManager>();
    }
    // TODO: Fully implement
    protected override void Tick()
    {
        storageScript.powerStored += amount;
        moneymanager.money -= moneyGained;
    }
}
