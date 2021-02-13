using System.Collections;
using UnityEngine;

public class ConsumerScript : PowerBase
{

    [Tooltip("How much power the object consumes per unit time")]
    public float consumeAmount;

    [Tooltip("Is the object currently consuming power. True is yes, false if no")]
    public bool isConsuming;

    [Tooltip("Is the object currently gaining power. True if yes, false if no")]
    public bool isPowered;

    // TODO: Fully implement
    protected override IEnumerator Tick()
    {
        yield return new WaitForSeconds(timestep);

        if (storage.powerStored >= consumeAmount)
        {
            storage.powerStored -= consumeAmount;
            isConsuming = true;
        }
    }
}
