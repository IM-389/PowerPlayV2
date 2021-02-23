using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralObjectScript : MonoBehaviour
{
    public enum Voltage {LOW, HIGH, TRANSFORMER }
    public Voltage volts;
    public bool isGenerator;
    public bool isConsumer;
    public bool isSubstation;
    public float wireLength;
    [Tooltip("Not used for placing, used for refunding after removal")]
    public int cost;
    
    
    public int maxConnectiions;
    
    public List<GameObject> connections = new List<GameObject>();
    public List<GameObject> consumerConnections = new List<GameObject>();


    public void AddConsumerConnection(GameObject connection)
    {
        consumerConnections.Add(connection);
    }
    public void AddConnection(GameObject connection)
    {
        connections.Add(connection);
    }
    public void RemoveConnection(GameObject connection)
    {
        connections.Remove(connection);
    }
    public int GetVoltage()
    {
        return (int)volts;
    }
}
