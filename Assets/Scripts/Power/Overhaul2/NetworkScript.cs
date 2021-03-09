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

        if (gos.isConsumer)
        {
            manager.powerConsumed += gameObject.GetComponent<PowerAmountInfo>().amount;
        } else if (gos.isGenerator)
        {
            manager.powerGenerated += gameObject.GetComponent<PowerAmountInfo>().amount;
        }

    }

    public void OnRemoved(GameObject removedFrom)
    {
        Debug.Log("Disconnected!");
        if (!FindManager(manager))
        {
            if (gos.isConsumer)
            {
                removedFrom.GetComponent<NetworkScript>().manager.powerConsumed -=
                    gameObject.GetComponent<PowerAmountInfo>().amount;
            } else if (gos.isGenerator)
            {
                removedFrom.GetComponent<NetworkScript>().manager.powerGenerated -=
                    gameObject.GetComponent<PowerAmountInfo>().amount;
            }

            isVisited = false;
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
        if (manager.precedenceNumber < otherManager.precedenceNumber)
        {
            manager.SetProperties(otherManager);
            otherNetwork.ChangeManager(manager);
            otherNetwork.isManager = false;
            Destroy(otherManager);
        }
        else if (!otherNetwork.isManager)
        {
            //Destroy(manager);
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
            NegotiateManager(allConnections[0].GetComponent<NetworkScript>());
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
        isVisited = true;
        if (isManager && manager.Equals(networkManager))
        {
            return true;
        }
        
        if (isVisited)
        {
            return false;
        }
        
        List<GameObject> allConnections = gos.GetAllConnections();

        bool found = false;
        foreach (var connection in allConnections)
        {
            found = connection.GetComponent<NetworkScript>().FindManager(networkManager);
            if (found)
            {
                break;
            }
        }
        
        return found;
    }
}
