using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

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
    
    
    [FormerlySerializedAs("maxConnectiions")] public int maxHVConnections;
    public int maxLVConnections;
    
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
        consumerConnections.Remove(connection);
        connections.RemoveAll(item => item == null);
        consumerConnections.RemoveAll(item => item == null);
    }
    public int GetVoltage()
    {
        return (int)volts;
    }
}
