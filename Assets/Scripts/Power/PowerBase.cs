using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StorageScript))]
[RequireComponent(typeof(GeneralObjectScript))]
[RequireComponent(typeof(TransmitterScript))]
public abstract class PowerBase : MonoBehaviour
{
    /// <summary>
    /// Current timestep value
    /// </summary>
    private float timestep;

    /// <summary>
    /// Timestep last frame
    /// </summary>
    private float previousTimestep;
    
    /// <summary>
    /// Reference to the PowerStorage component
    /// </summary>
    protected StorageScript storageScript;

    /// <summary>
    /// Reference to the TimeManager. Used to detect timestep changes
    /// </summary>
    protected TimeManager timeManager;

    /// <summary>
    /// Reference to the attached GeneralObjectScript. Used for connections
    /// </summary>
    protected GeneralObjectScript gos;
    
    /// <summary>
    /// Get the fixed timestep from the TimeManager
    /// </summary>
    private void Start()
    {
        storageScript = gameObject.GetComponent<StorageScript>();
        timeManager = GameObject.FindWithTag("GameController").GetComponent<TimeManager>();
        gos = gameObject.GetComponent<GeneralObjectScript>();
    }

    /// <summary>
    /// Detect a change in timestep and tick the objects
    /// </summary>
    private void Update()
    {
        timestep = timeManager.totalTimeSteps;
        if (timestep > previousTimestep)
        {
            Tick();
        }
    
        previousTimestep = timeManager.totalTimeSteps;
    }
    
    /// <summary>
    /// Run the object, consuming power if it is on.
    /// Runs at a fixed timestep dependent on the global time
    /// </summary>
    /// <returns></returns>
    protected abstract void Tick();
}
