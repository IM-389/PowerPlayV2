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
    public float wireLength;
    public string buildingText;
    //public string wireText;
    [FormerlySerializedAs("cost")] [Tooltip("Not used for placing, used for refunding after removal")]
    public int refundAmount;

    [Tooltip("Object showing the range of the object when hovered over")]
    public GameObject buildCircle;
    
    /// <summary>
    /// Determines whether object is removable or not
    /// </summary>
    public bool unRemovable;
    bool destroyed = false;
    GameObject destroyKey;
    
    [FormerlySerializedAs("maxConnectiions")] public int maxHVConnections;
    public int maxLVConnections;
    

    public List<GameObject> hVConnections = new List<GameObject>();
    public List<GameObject> lvConnections = new List<GameObject>();
    public List<GameObject> consumerConnections = new List<GameObject>();
        
    [Tooltip("Does this object count towards milestone progress")]
    public bool isMilestoneCounted = true;
    
    //public List<GameObject> connections = new List<GameObject>();
    //public List<GameObject> consumerConnections = new List<GameObject>();
    
    public Dictionary<GameObject, GameObject> wireConnections = new Dictionary<GameObject, GameObject>();

    public GameObject[] preMadeConnections;

    // Makes premade connections
    private void Start()
    {
        buildCircle.transform.localScale *= wireLength;
        foreach(GameObject connection in preMadeConnections)
        {
            GeneralObjectScript gos = connection.GetComponent<GeneralObjectScript>();
            if(gos.volts == Voltage.LOW)
            {
                AddLVConnection(connection);
            }
            else if (gos.volts == Voltage.HIGH)
            {
                AddHVConnection(connection);
            }
            else if(gos.volts == Voltage.TRANSFORMER)
            {
                if(this.volts == Voltage.HIGH)
                {
                    AddHVConnection(connection);
                }
                else if(this.volts == Voltage.LOW)
                {
                    AddLVConnection(connection);
                }
                else
                {
                    AddLVConnection(connection);
                }
            }

        }
    }
    public void AddLVConnection(GameObject connection)
    {
        lvConnections.Add(connection);
        CreateLine(connection, Color.white, 0.1f);
    }
    public void AddHVConnection(GameObject connection)
    {
        hVConnections.Add(connection);
        CreateLine(connection, Color.white, 0.1f);
    }
    public void AddConsumerConnection(GameObject connection)
    {
        consumerConnections.Add(connection);
        CreateLine(connection, Color.black, 0.05f);
    }
    public void RemoveConnection(GameObject connection)
    {
        hVConnections.Remove(connection);
        lvConnections.Remove(connection);
        consumerConnections.Remove(connection);
        hVConnections.RemoveAll(item => item == null);
        lvConnections.RemoveAll(item => item == null);
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
    public void CreateLine(GameObject connection, Color color, float width)
    {
        Vector3 thisPos = this.transform.position;
        Vector3 connectPos = connection.transform.position;
        // Creates line
        GameObject myLine = new GameObject();
        myLine.name = "powerLine";
        
        myLine.transform.position = connectPos + new Vector3(0, 0, 0.5f);
        myLine.AddComponent<LineRenderer>();
        myLine.AddComponent<WireScript>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        WireScript ws = myLine.GetComponent<WireScript>();
        ws.connect1 = this.gameObject;
        ws.connect2 = connection;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.SetPosition(0, thisPos);
        lr.SetPosition(1, connectPos);

        // Creates box collider to wire
        BoxCollider2D col = new GameObject("Collider").AddComponent<BoxCollider2D>();
        col.transform.parent = myLine.transform;
        col.transform.tag = "wire";
        col.transform.gameObject.layer = 8;
        float lineLength = Vector2.Distance(new Vector2(thisPos.x, thisPos.y), new Vector2(connectPos.x,connectPos.y));
        Debug.Log(lineLength);
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

    /// <summary>
    /// Get all connections attached to the object
    /// </summary>
    /// <returns>A list of all connections</returns>
    public List<GameObject> GetAllConnections()
    {
        List<GameObject> allConnections = new List<GameObject>();
        allConnections.AddRange(hVConnections);
        allConnections.AddRange(lvConnections);
        allConnections.AddRange(consumerConnections);

        return allConnections;
    }

    /// <summary>
    /// Get total number of connections an object has
    /// </summary>
    /// <returns>The number of connections</returns>
    public int GetAllConnectionsCount()
    {
        return (hVConnections.Count + lvConnections.Count + consumerConnections.Count);
    }
}
