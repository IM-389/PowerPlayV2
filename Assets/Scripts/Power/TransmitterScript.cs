using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmitterScript : PowerBase
{
    [Tooltip("If the object is allowed to send power. True is yes, false is no")]
    public bool canSend = true;

    [Tooltip("If the object is allowed to recieve power. True if yes, false if no")]
    public bool canRecieve = true;

    private bool isLVPole = false;

    public int priorityNumber;
    protected override void Start()
    {
        base.Start();
        if (gos.GetType() == typeof(LVPowerLine))
        {
            isLVPole = true;
        }
        
    }
    
    protected override void Tick()
    {
        if (!canSend)
        {
            return;
        }

        List<TransmitterScript> toTransmit = new List<TransmitterScript>();
        // The most the object can push is limited so each connected object gets the same total power.

        toTransmit.AddRange(AddDestinations(gos.nonConsumerConnections));
        toTransmit.AddRange(AddDestinations(gos.consumerConnections));
        
        /*
        if (isLVPole)
        {
            foreach (var destination in gos.consumerConnections)
            {
                StorageScript otherStorage = destination.GetComponent<StorageScript>();
                TransmitterScript otherTransmitter = destination.GetComponent<TransmitterScript>();
                if (otherTransmitter.canRecieve && !otherStorage.isFull)
                {
                    toTransmit.Add(otherTransmitter);
                }
            }
        }
        */
        
        
        float maxPush = storageScript.powerStored / toTransmit.Count;
        // Send power to those with less power than the sender 
        foreach (var destination in toTransmit)
        {
            //Debug.Log($"Transmitting to {destination.name}!");
            // This object asks the connected object to pull power
            StartCoroutine(destination.ReceivePower(this, maxPush));
        }

    }
    
    /// <summary>
    /// Receive power pushed from another transmitter
    /// </summary>
    /// <param name="source">The transmitter that pushed the power</param>>
    /// <param name="amount">How much power was received</param>
    private IEnumerator ReceivePower(TransmitterScript source, float amount)
    {
        yield return new WaitForSeconds(0.1f);
        // Prevent object from overfilling
        if (storageScript.isFull)
        {
            yield break;
        }
        // Pulling entire amount from the source
        source.storageScript.PullPower(amount);
        // Adding the amount to the reciever (this object)
        storageScript.PushPower(amount);
    }

    private List<TransmitterScript> AddDestinations(List<GameObject> source)
    {
        List<TransmitterScript> toTransmit = new List<TransmitterScript>();
        foreach (var destination in source)
        {
            // Prevent power from being pushed to null connections
            if (destination == null)
            {
                continue;
            }

            StorageScript otherStorage = destination.GetComponent<StorageScript>();
            if (storageScript.powerStored >= otherStorage.powerStored)
            {
                TransmitterScript otherTransmitter = destination.GetComponent<TransmitterScript>();
                if (otherTransmitter.canRecieve && !otherStorage.isFull 
                    && priorityNumber <= otherTransmitter.priorityNumber)
                {
                    toTransmit.Add(otherTransmitter);
                }
            }
        }

        return toTransmit;
    }
}
