using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the calculations for all connected objects. Which object is the manager is determined by precedence numbers
/// </summary>
[RequireComponent(typeof(GeneralObjectScript))]
public class NetworkScript : MonoBehaviour
{

    [Tooltip("The current manager. Starts as self, changes based on precedence when connected")]
    public NetworkManager manager;

    /// <summary>
    /// Reference to this object's GeneralObjectScript
    /// </summary>
    public GeneralObjectScript gos;

    public bool isManager = true;

    private bool isVisited = false;

    private List<NetworkManager> visitedManagers = new List<NetworkManager>();
    
    /*
     * Dictionary of if each connection leads to the generator
     * Pathfinding algo style
     * excluding myself, does this take me closer to the generator (in # of connections)
     *  as you find, only propogate to something with a lower stepcount
     *  when new manager is promoted, propogate the update step count
     */
    // Start is called before the first frame update
    void Awake()
    {
        // Each object starts as its own manager
        manager = gameObject.GetComponent<NetworkManager>();
        
        gos = gameObject.GetComponent<GeneralObjectScript>();

    }
    
    
    /// <summary>
    /// Called when connecting an object to another one
    /// </summary>
    public void OnConnected(GameObject connectedTo)
    {
        NetworkScript otherNetwork = connectedTo.GetComponent<NetworkScript>();

        NegotiateManager(otherNetwork);

        manager.powerConsumed += gameObject.GetComponent<PowerAmountInfo>().amountConsumed;
        
        manager.powerGenerated += gameObject.GetComponent<PowerAmountInfo>().amountGenerated;

    }

    public void OnRemoved(GameObject removedFrom)
    {
        Debug.Log("Disconnected!");
        if (!FindManager(manager))
        {
            manager.powerConsumed -= gameObject.GetComponent<PowerAmountInfo>().amountConsumed;
            manager.powerGenerated -= gameObject.GetComponent<PowerAmountInfo>().amountGenerated;
            manager = null;
            //isVisited = false;
            visitedManagers.Clear();
            Debug.Log($"Promoting {gameObject.name} to manager!");
            PromoteManager();
        }
    }

    /// <summary>
    /// Determine which object should be the manager
    /// </summary>
    /// <param name="otherNetwork">The network to compare against</param>
    /// <returns></returns>
    private void NegotiateManager(NetworkScript otherNetwork)
    {
        NetworkManager otherManager = otherNetwork.manager;

        if (otherManager == manager)
        {
            return;
        }
        
        Debug.Log($"Negotiating between {manager.gameObject.name} and {otherManager.gameObject.name}");

        // This always runs at least once per connection
        if (manager.precedenceNumber < otherManager.precedenceNumber)
        {
            //manager.SetProperties(otherManager);
            //otherNetwork.ChangeManager(manager);
            Debug.Log($"{manager.name} has lower predecende");
            otherNetwork.isManager = false;
            //DestroyImmediate(otherManager);
        }
        // This may run: either this or the else below run
        else if (manager.precedenceNumber == otherManager.precedenceNumber)
        {
            Debug.Log("Managers share precedence, increasing number");
            DestroyImmediate(manager);
            //++manager.precedenceNumber;
        }
        else
        {
            Debug.Log($"{otherManager} has higher precedence, updating manager");
            otherManager.SetProperties(manager);
            DestroyImmediate(manager);
            ChangeManager(otherManager);
        }
        
        
    }

    /// <summary>
    /// Updates the manager for all objects on the network
    /// </summary>
    /// <param name="newManager">New manager for the network</param>
    private void ChangeManager(NetworkManager newManager)
    {
        if (manager == newManager)
        {
            return;
        }

        isManager = false;
        manager = newManager;

        List<GameObject> allConnections = gos.GetAllConnections();

        foreach (var connection in allConnections)
        {
            connection.GetComponent<NetworkScript>().ChangeManager(newManager);
        }
    }


    /// <summary>
    /// Promotes the object to a manager, then negotiates with the other manager on the network
    /// </summary>
    private void PromoteManager()
    {
        manager = gameObject.AddComponent<NetworkManager>();
        isManager = true;
        List<GameObject> allConnections = gos.GetAllConnections();
        if (allConnections.Count > 0)
        {
            allConnections[0].GetComponent<NetworkScript>().ChangeManager(manager);
        }
    }

    /// <summary>
    /// Searches all connected objects for the given manager
    /// </summary>
    /// <param name="networkManager">The manager to search for</param>
    /// <returns>True if manager is found, false if not</returns>
    private bool FindManager(NetworkManager networkManager)
    {
        Debug.Log($"Finding {networkManager.gameObject.name} from {gameObject.name}");
        if (isManager && manager.Equals(networkManager))
        {
            Debug.Log($"{gameObject.name} found manager!!!");
            visitedManagers.Clear();
            return true;
        }
        
        //if (isVisited)
        if (visitedManagers.Contains(networkManager))
        {
            Debug.Log($"{gameObject.name} already visited {networkManager.gameObject.name}!");
            return false;
        }
        
        //isVisited = true;
        visitedManagers.Add(networkManager);
        List<GameObject> allConnections = gos.GetAllConnections();

        bool found = false;
        foreach (var connection in allConnections)
        {
            found = connection.GetComponent<NetworkScript>().FindManager(networkManager);
            if (found)
            {
                Debug.Log($"{gameObject.name} found manager, breaking!!!");
                break;
            }
        }
        
        visitedManagers.Clear();
        return found;
    }
}
