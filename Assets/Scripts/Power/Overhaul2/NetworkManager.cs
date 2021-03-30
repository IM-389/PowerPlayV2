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
    public float powerGenerated;
    
    /// <summary>
    /// How much power is being consumed in the connected network
    /// </summary>
    public float powerConsumed;

    public bool hasEnoughPower = false;

    // Start is called before the first frame update
    void Awake()
    {
        powerConsumed += gameObject.GetComponent<PowerAmountInfo>().amountConsumed;
        
        powerGenerated += gameObject.GetComponent<PowerAmountInfo>().amountGenerated;
        // Determine predecence number randomly upon placement
        // Random number is large enough to (hopefully) prevent 2 managers with the same from being connected
        precedenceNumber = Random.Range(0, 100000000);
    }

    private void Update()
    {
        hasEnoughPower = powerConsumed <= powerGenerated;
    }
    
    public void SetProperties(NetworkManager other)
    {
        powerConsumed += other.powerConsumed;
        powerGenerated += other.powerGenerated;
    }
}
