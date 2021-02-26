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
        CreateLine(connection);
    }
    public void AddConnection(GameObject connection)
    {
        connections.Add(connection);
        CreateLine(connection);
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
    public void CreateLine(GameObject connection)
    {
        Vector3 thisPos = this.transform.position;
        Vector3 connectPos = connection.transform.position;
        // Creates line
        GameObject myLine = new GameObject();
        myLine.name = "powerLine";
        myLine.transform.position = connectPos;
        myLine.AddComponent<LineRenderer>();
        myLine.AddComponent<WireScript>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        WireScript ws = myLine.GetComponent<WireScript>();
        ws.connect1 = this.gameObject;
        ws.connect2 = connection;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = .02f;
        lr.endWidth = .02f;
        lr.SetPosition(0, thisPos);
        lr.SetPosition(1, connectPos);

        // Creates box collider to wire
        BoxCollider2D col = new GameObject("Collider").AddComponent<BoxCollider2D>();
        col.transform.parent = myLine.transform;
        col.transform.tag = "wire";
        float lineLength = Vector3.Distance(thisPos, connectPos);
        col.size = new Vector3(lineLength, 0.1f, 1f);
        Vector3 midPoint = (thisPos + connectPos) / 2;
        col.transform.position = midPoint;
        float angle = (Mathf.Abs(thisPos.y - connectPos.y) / Mathf.Abs(thisPos.x - connectPos.x));
        if ((thisPos.y < connectPos.y && thisPos.x > connectPos.x) || (connectPos.y < thisPos.y && connectPos.x > thisPos.x))
        {
            angle *= -1;
        }
        angle = Mathf.Rad2Deg * Mathf.Atan(angle);
        col.transform.Rotate(0, 0, angle);

        wireConnections.Add(connection, myLine);
    }
}
