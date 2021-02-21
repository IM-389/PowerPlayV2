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
    private PowerManager powerManager;
    void Start()
    {
        base.Start();
        moneymanager = GameObject.FindWithTag("GameController").GetComponent<MoneyManager>();
        powerManager = GameObject.FindWithTag("GameController").GetComponent<PowerManager>();
    }
    // TODO: Fully implement
    protected override void Tick()
    {
        float amountModified = amount;
        if (type == PowerManager.POWER_TYPES.TYPE_SOLAR)
        {
            if (!timeManager.isDay)
            {
                return;
            }
        }

        for (int i = 0; i < 4; ++i)
        {
            Debug.Log($"{gameObject.name} type int {(int)type}");
            if ((int)type == i)
            {
                amountModified *= powerManager.powerAdjusts[i];
                break;
            }
        }
        
        storageScript.powerStored += amountModified;
        moneymanager.money -= moneyGained;
    }

    /// <summary>
    /// Disable the generator for a given amount of time
    /// </summary>
    /// <param name="seconds">How many seconds to disable it for</param>
    /// <returns></returns>
    public IEnumerator TimedDisable(int seconds)
    {
        float orignialAmount = amount;
        amount = 0;

        yield return new WaitForSeconds(seconds);

        amount = orignialAmount;
    }
}
