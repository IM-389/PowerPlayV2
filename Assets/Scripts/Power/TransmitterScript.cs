using System.Collections.Generic;
using UnityEngine;

public class TransmitterScript : PowerBase
{
    [Tooltip("If the object is allowed to send power. True is yes, false is no")]
    public bool canSend = true;
    protected override void Tick()
    {
        if (!canSend)
        {
            return;
        }

        List<TransmitterScript> toTransmit = new List<TransmitterScript>();
        // The most the object can push is limited so each connected object gets the same total power.

        // Filter to all objects with less power than this one. Allows for bidirectional wires
        foreach (var destination in gos.connections)
        {
            // Prevent power from being pushed to null connections
            if (destination == null)
            {
                continue;
            }
            
            StorageScript otherStorage = destination.GetComponent<StorageScript>();

            if (storageScript.powerStored >= otherStorage.powerStored)
            {
                toTransmit.Add(destination.GetComponent<TransmitterScript>());
            }
        }

        float maxPush = storageScript.powerStored / toTransmit.Count;
        // Send power to those with less power than the sender 
        foreach (var destination in toTransmit)
        {
            // This object asks the connected object to pull power
            destination.ReceivePower(this, maxPush);
        }

    }
    
    /// <summary>
    /// Receive power pushed from another transmitter
    /// </summary>
    /// <param name="source">The transmitter that pushed the power</param>>
    /// <param name="amount">How much power was received</param>
    private void ReceivePower(TransmitterScript source, float amount)
    {
        // Prevent object from overfilling
        if (storageScript.isFull)
        {
            return;
        }
        // Pulling entire amount from the source
        source.storageScript.PullPower(amount);
        // Adding the amount to the reciever (this object)
        storageScript.PushPower(amount);
    }
}
