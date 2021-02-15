using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralObjectScript : MonoBehaviour
{
    public List<GameObject> connections = new List<GameObject>();

    public void AddConnection(GameObject connection)
    {
        connections.Add(connection);
    }
    public void RemoveConnection(GameObject connection)
    {
        connections.Remove(connection);
    }
}
