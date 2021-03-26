using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
    public bool connected = false;
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
    
    [FormerlySerializedAs("maxHVConnections")] [FormerlySerializedAs("maxConnectiions")] public int maxConnections;
    //public int maxLVConnections;
    

    [FormerlySerializedAs("hVConnections")] public ObservableCollection<GameObject> nonConsumerConnections = new ObservableCollection<GameObject>();
    //public List<GameObject> lvConnections = new List<GameObject>();
    public ObservableCollection<GameObject> consumerConnections = new ObservableCollection<GameObject>();
    [Tooltip("Does this object count towards milestone progress")]
    public bool isMilestoneCounted = true;
    
    //public List<GameObject> connections = new List<GameObject>();
    //public List<GameObject> consumerConnections = new List<GameObject>();
    
    public Dictionary<GameObject, GameObject> wireConnections = new Dictionary<GameObject, GameObject>();

    public GameObject[] preMadeConnections;

    [Tooltip("How many connections the object has")]
    public int connectionCount;

    /// <summary>
    /// Reference to the attached network script
    /// </summary>
    protected NetworkScript powerNetwork;
    
    // Makes premade connections
    private void Start()
    {
        // Register the list changing to the callback function.
        hVConnections.CollectionChanged += OnListChanged;
        lvConnections.CollectionChanged += OnListChanged;
        consumerConnections.CollectionChanged += OnListChanged;

        powerNetwork = gameObject.GetComponent<NetworkScript>();
        
        foreach(GameObject connection in preMadeConnections)
        {
            //GeneralObjectScript gos = connection.GetComponent<GeneralObjectScript>();
            /*
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
            */
            AddNonConsumerConnection(connection);

        }
    }
    /*
    public void AddLVConnection(GameObject connection)
    {
        lvConnections.Add(connection);
        ++connectionCount;
        connected = true;
        CreateLine(connection, Color.white, 0.1f);
    }
    */
    public void AddNonConsumerConnection(GameObject connection)
    {
        nonConsumerConnections.Add(connection);
        connected = true;
        CreateLine(connection, Color.white, 0.1f);
    }
    public void AddConsumerConnection(GameObject connection)
    {
        consumerConnections.Add(connection);
        ++connectionCount;
        connected = true;
        CreateLine(connection, Color.black, 0.05f);
    }
    public void RemoveConnection(GameObject connection)
    {
        nonConsumerConnections.Remove(connection);
        //lvConnections.Remove(connection);
        consumerConnections.Remove(connection);
        nonConsumerConnections.RemoveAll(item => item == null);
        //lvConnections.RemoveAll(item => item == null);
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

        --connectionCount;
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
    /// Called whenever a change happens to the connected lists.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    protected void OnListChanged(object sender, NotifyCollectionChangedEventArgs args)
    {
        if (args.OldItems is null)
        {
            Debug.Log(args.NewItems[0]);
            powerNetwork.OnConnected((GameObject) args.NewItems[0]);
        }
        else
        {
            powerNetwork.OnRemoved((GameObject) args.OldItems[0]);
        }
    }
    
    /// Get all connections attached to the object
    /// </summary>
    /// <returns>A list of all connections</returns>
    public List<GameObject> GetAllConnections()
    {
        List<GameObject> allConnections = new List<GameObject>();
        allConnections.AddRange(nonConsumerConnections);
        //allConnections.AddRange(lvConnections);
        allConnections.AddRange(consumerConnections);

        return allConnections;
    }

    /// <summary>
    /// Get total number of connections an object has
    /// </summary>
    /// <returns>The number of connections</returns>
    public int GetAllConnectionsCount()
    {
        return (nonConsumerConnections.Count + consumerConnections.Count);
    }

    public void OnMouseEnter()
    {
        gameObject.GetComponent<HoverScript>().ToggleBuildCircle(true);
    }

    public void OnMouseExit()
    {
        gameObject.GetComponent<HoverScript>().ToggleBuildCircle(false);
    }
}
