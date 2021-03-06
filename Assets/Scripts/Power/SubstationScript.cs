using UnityEngine;

public class SubstationScript : MonoBehaviour
{

    private GeneralObjectScript gos;

    public bool voltageSet;
    
    [Tooltip("How many connections the substation can have")]
    public int maxConnections;
    
    // Start is called before the first frame update
    void Start()
    {
        gos = gameObject.GetComponent<GeneralObjectScript>();
    }

    private void Update()
    {
        if (gos.hVConnections.Count <= 0 && gos.hVConnections.Count <= 0 )
        {
            voltageSet = false;
            gos.maxLVConnections = 1;
            gos.maxHVConnections = 1;
        }
        
        if (!voltageSet)
        {
            if (gos.hVConnections.Count > 0)
            {
                gos.volts = GeneralObjectScript.Voltage.HIGH;
                gos.maxHVConnections = maxConnections;
                gos.maxLVConnections = 0;
                voltageSet = true;
            }
            else if (gos.lvConnections.Count > 0)
            {
                gos.volts = GeneralObjectScript.Voltage.LOW;
                gos.maxLVConnections = maxConnections;
                gos.maxHVConnections = 0;
                voltageSet = true;
            }
            
        }
        
    }
}

