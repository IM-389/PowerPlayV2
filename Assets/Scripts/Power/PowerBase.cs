using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PowerStorage))]
public abstract class PowerBase : MonoBehaviour
{
    /// <summary>
    /// Fixed timestep that all power objects operate on
    /// </summary>
    protected float timestep;

    /// <summary>
    /// Reference to the PowerStorage component
    /// </summary>
    protected PowerStorage storage;
    
    /// <summary>
    /// Get the fixed timestep from the TimeManager
    /// </summary>
    private void Start()
    {
        // TODO: Pull from TimeManager
        timestep = 0.5f;

        storage = gameObject.GetComponent<PowerStorage>();
    }

    /// <summary>
    /// Run the object, consuming power if it is on.
    /// Runs at a fixed timestep dependent on the global time
    /// </summary>
    /// <returns></returns>
    protected abstract IEnumerator Tick();
}
