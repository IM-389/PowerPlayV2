using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralObjectScript : MonoBehaviour
{
    public enum Voltage {LOW, HIGH, TRANSFORMER }
    public Voltage volts;
    public bool isGenerator;
    public bool isConsumer;

    public List<GameObject> connections = new List<GameObject>();

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
