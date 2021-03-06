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
    public string buildingText;
    //public string wireText;
    [Tooltip("Not used for placing, used for refunding after removal")]
    public int cost;

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

        
    [Tooltip("Does this object count towards milestone progress")]
    public bool isMilestoneCounted = true;
    
    //public List<GameObject> connections = new List<GameObject>();
    //public List<GameObject> consumerConnections = new List<GameObject>();
    
    public Dictionary<GameObject, GameObject> wireConnections = new Dictionary<GameObject, GameObject>();

    public GameObject[] preMadeConnections;

    // Makes premade connections
    private void Start()
    {
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
        CreateLine(connection);
    }
    public void AddHVConnection(GameObject connection)
    {
        hVConnections.Add(connection);
        CreateLine(connection);
    }
    public void RemoveConnection(GameObject connection)
    {
        hVConnections.Remove(connection);
        lvConnections.Remove(connection);
        hVConnections.RemoveAll(item => item == null);
        lvConnections.RemoveAll(item => item == null);
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
        
        myLine.transform.position = connectPos + new Vector3(0, 0, 0.5f);
        myLine.AddComponent<LineRenderer>();
        myLine.AddComponent<WireScript>();
        LineRenderer lr = myLine.GetComponent<LineRenderer>();
        WireScript ws = myLine.GetComponent<WireScript>();
        ws.connect1 = this.gameObject;
        ws.connect2 = connection;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = Color.white;
        lr.endColor = Color.white;
        lr.startWidth = .1f;
        lr.endWidth = .1f;
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
}
