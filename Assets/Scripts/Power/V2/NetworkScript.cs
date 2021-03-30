using System;
using System.Collections.Generic;
using UnityEngine;

namespace Power.V2
{
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

        private List<GameObject> visitedObjects = new List<GameObject>();

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

        }

        public void OnRemoved(GameObject removedFrom)
        {
            Debug.Log("Disconnected!");
            if (!FindManager(manager, gameObject))
            {
                NetworkManager prevManager = manager;
                manager = null;
                //isVisited = false;
                visitedObjects.Clear();
                Debug.Log($"Promoting {gameObject.name} to manager!");
                PromoteManager();
                prevManager.powerConsumed -= manager.powerConsumed;
                prevManager.powerGenerated -= manager.powerGenerated;
            }
        }

        /// <summary>
        /// Determine which object should be the manager
        /// </summary>
        /// <param name="otherNetwork">The network to compare against</param>
        /// <returns></returns>
        private bool NegotiateManager(NetworkScript otherNetwork)
        {
            NetworkManager otherManager = otherNetwork.manager;

            if (otherManager == manager)
            {
                return false;
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
            // TODO: Fix this, things still break really badly if numbers are equal
            else if (manager.precedenceNumber == otherManager.precedenceNumber)
            {
                Debug.Log("Managers share precedence, increasing number");
                DestroyImmediate(manager);
                //++manager.precedenceNumber;
                return false;
            }
            else
            {
                Debug.Log($"{otherManager} has higher precedence, updating manager");
                //otherManager.SetProperties(manager);
                DestroyImmediate(manager);
                ChangeManager(otherManager);
            }

            return true;


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

            manager.powerConsumed += gameObject.GetComponent<PowerAmountInfo>().amountConsumed;
            manager.powerGenerated += gameObject.GetComponent<PowerAmountInfo>().amountGenerated;

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
            foreach (var connection in allConnections)
            {
                connection.GetComponent<NetworkScript>().ChangeManager(manager);
            }
        }

        /// <summary>
        /// Searches all connected objects for the given manager
        /// </summary>
        /// <param name="networkManager">The manager to search for</param>
        /// <returns>True if manager is found, false if not</returns>
        private bool FindManager(NetworkManager networkManager, GameObject target)
        {
            Debug.Log($"Finding {networkManager.gameObject.name} from {gameObject.name}");
            if (isManager && manager.Equals(networkManager))
            {
                Debug.Log($"FindManager: {gameObject.name} found manager!!!");
                visitedObjects.Clear();
                return true;
            }

            //if (isVisited)
            if (visitedObjects.Contains(target))
            {
                Debug.Log($"FindManager: {gameObject.name} already visited {networkManager.gameObject.name}!");
                return false;
            }

            //isVisited = true;
            List<GameObject> allConnections = gos.GetAllConnections();

            bool found = false;
            foreach (var connection in allConnections)
            {
                visitedObjects.Add(connection);
                found = connection.GetComponent<NetworkScript>().FindManager(networkManager, gameObject);
                //found = FindManager(networkManager, connection);
                if (found)
                {
                    Debug.Log($"FindManager: {gameObject.name} found manager, breaking!!!");
                    break;
                }
            }

            visitedObjects.Clear();
            return found;
        }
    }
}
