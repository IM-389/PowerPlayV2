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
    
    /// <summary>
    /// How much power to consume, adjusted from the curve
    /// </summary>
    private float consumeAmount;
    
    
    // TODO: Fully implement
    protected override void Tick()
    {
        // TODO: Smooth the transition between values
        consumeAmount = consumptionCurve[timeManager.hours];
        // If there is enough power, consume it
        if ((storageScript.powerStored >= consumeAmount))
        {
            storageScript.powerStored -= consumeAmount;
            // Set the flag so other objects can know if this one is consuming
            isConsuming = true;
        }
        else
        {
            // Set the flag so other objects can know if this one is consuming
            isConsuming = false;
        }
    }
}
