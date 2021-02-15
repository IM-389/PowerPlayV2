public class TransmitterScript : PowerBase
{
    protected override void Tick()
    {
        
        // The most the object can push is limited so each connected object gets the same total power.
        float maxPush = storageScript.powerStored / gos.connections.Count;
        foreach (var destination in gos.connections)
        {
            // Prevent power from being pushed to null connections
            if (destination == null)
            {
                continue;
            }
            // This object asks the connected object to pull power
            destination.GetComponent<TransmitterScript>().ReceivePower(this, maxPush);
        }

    }
    
    /// <summary>
    /// Receive power pushed from another transmitter
    /// </summary>
    /// <param name="source">The transmitter that pushed the power</param>>
    /// <param name="amount">How much power was received</param>
    private void ReceivePower(TransmitterScript source, float amount)
    {
        // Pulling entire amount due to efficiency losses
        source.storageScript.PullPower(amount);
        // Adding the adjusted amount due to efficiency losses
        storageScript.PushPower(amount);
    }
}
