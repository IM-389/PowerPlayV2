using System.Collections;
using UnityEngine;

public class ConsumerScript : PowerBase
{

    [Tooltip("Is the object currently consuming power. True is yes, false if no")]
    public bool isConsuming;

    [Tooltip("Is the object currently gaining power. True if yes, false if no")]
    public bool isPowered;

    [Tooltip("How the consumption changes over time. Index is hour, value is consumption at that hour")]
    public float[] consumptionCurve = new float[24];

    public AnimationCurve consumptionCurve2;

    /// <summary>
    /// How much power to consume, adjusted from the curve
    /// </summary>
    private float consumeAmount;

    private float consumeAmount2;

    /// <summary>
    /// If the object is cut off from power. Set to true the tick the object does not have enough power
    /// Used to delay setting the isConsuming flag to give leeway with short power drops
    /// </summary>
    private bool isCutOff = false;
    
    private MoneyManager moneymanager;
    public TimeManager cityApproval;
    public int moneyGained;
    void Start()
    {
        base.Start();
        moneymanager = GameObject.FindWithTag("GameController").GetComponent<MoneyManager>();
    }
    
    // TODO: Fully implement
    protected override void Tick()
    {
        // TODO: Smooth the transition between values
        //consumeAmount = consumptionCurve[timeManager.hours];
        consumeAmount = consumptionCurve2.Evaluate(timeManager.hours);
        
        Debug.Log(timeManager.hours);
        Debug.Log("Consume Amount 1:" + consumeAmount);
        Debug.Log("Consume Amount 2:" + consumeAmount2);
        // If there is enough power, consume it

        if ((storageScript.powerStored >= consumeAmount))
        {
            storageScript.PullPower(consumeAmount);
            // Set the flag so other objects can know if this one is consuming
            isConsuming = true;
            isCutOff = false;
            moneymanager.money += moneyGained;

        }
        else
        {
            // Set the flag so other objects can know if this one is consuming
            isCutOff = !isConsuming;
            isConsuming = !isCutOff;
        }


        if (cityApproval != null)
        {
            if (cityApproval.hours == 25)
            {
                if (!isConsuming)
                {
                    cityApproval.cityApproval -= 20;//we're gonna hope this works
                    Debug.Log("City approval should go down at the end of the day");
                }

            }
        }
        
       

        if (gos.GetAllConnectionsCount() == 0)
        {
            storageScript.powerStored = 0;
        }
    }
}
