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

    bool destroyed = false;
    GameObject destroyKey;
    
    [FormerlySerializedAs("maxConnectiions")] public int maxHVConnections;
    public int maxLVConnections;
    
    public List<GameObject> connections = new List<GameObject>();
    public List<GameObject> consumerConnections = new List<GameObject>();
    public Dictionary<GameObject, GameObject> wireConnections = new Dictionary<GameObject, GameObject>();


    public void AddConsumerConnection(GameObject connection)
    {
        consumerConnections.Add(connection);
        // Creates line
        GameObject myLine = new GameObject();
        myLine.name = "powerLine";
        myLine.transform.position = connection.transform.position;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = .02f;
        lr.endWidth = .02f;
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, connection.transform.position);
        wireConnections.Add(connection, myLine);
    }
    public void AddConnection(GameObject connection)
    {
        connections.Add(connection);
        // Creates line
        GameObject myLine = new GameObject();
        myLine.name = "powerLine";
        myLine.transform.position = connection.transform.position;
        myLine.AddComponent<LineRenderer>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = .02f;
        lr.endWidth = .02f;
        lr.SetPosition(0, this.transform.position);
        lr.SetPosition(1, connection.transform.position);
        wireConnections.Add(connection, myLine);
    }
    public void RemoveConnection(GameObject connection)
    {
        connections.Remove(connection);
        consumerConnections.Remove(connection);
        connections.RemoveAll(item => item == null);
        consumerConnections.RemoveAll(item => item == null);
        foreach(KeyValuePair<GameObject, GameObject> kvp in wireConnections)
        {
            if (kvp.Key.Equals(connection))
            {
                Destroy(kvp.Value);
                destroyed = true;
                destroyKey = kvp.Key;
            }
        }
        if (destroyed)
        {
            wireConnections.Remove(destroyKey);
        }
    }
    public int GetVoltage()
    {
        return (int)volts;
    }
}
