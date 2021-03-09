using UnityEngine;

[RequireComponent(typeof(NetworkScript))]
public class NetworkManager : MonoBehaviour
{

    /// <summary>
    /// Used to determine which object gets manager when connecting. Lower numbers take priority
    /// In a tie, favor the manager of the bigger network
    /// </summary>
    public int precedenceNumber;

    /// <summary>
    /// How much power is being generated in the connected network
    /// </summary>
    public int powerGenerated;
    
    /// <summary>
    /// How much power is being consumed in the connected network
    /// </summary>
    public int powerConsumed;

    // Start is called before the first frame update
    void Start()
    {
        // Determine predecence number randomly upon placement
        precedenceNumber = Random.Range(0, 100);
    }

    public void SetProperties(NetworkManager other)
    {
        powerConsumed += other.powerConsumed;
        powerGenerated += other.powerGenerated;
    }
}
