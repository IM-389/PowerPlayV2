using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HoverScript : MonoBehaviour
{

    /// <summary>
    /// Reference to the tooltip panel
    /// </summary>
    private GameObject tooltipPanel;

    /// <summary>
    /// Reference to the text object showing power consumption
    /// </summary>
    private Text powerAmtText;

    /// <summary>
    /// Reference to this object's GeneralObjectScript
    /// </summary>
    private GeneralObjectScript gos;

    /// <summary>
    /// Reference to the TimeManager
    /// </summary>
    private TimeManager timeManager;

    /// <summary>
    /// Reference to this object's storage component
    /// </summary>
    private StorageScript storage;

    [Tooltip("Where to locate the tooltip relative to the object")]
    public Vector2 tooltipOffset;

    [Tooltip("If the object is equipped to be Smart. Provides more detailed info if yes")]
    public bool isSmart;
    
    private void Start()
    {
        tooltipPanel = GameObject.FindWithTag("TooltipPanel");
        powerAmtText = tooltipPanel.transform.GetChild(0).GetComponent<Text>();
        gos = gameObject.GetComponent<GeneralObjectScript>();
        timeManager = GameObject.FindWithTag("GameController").GetComponent<TimeManager>();
        storage = gameObject.GetComponent<StorageScript>();
    }
    
    
    public void UpdateTooltip()
    {
        Vector2 panelPos = transform.position;
        panelPos += tooltipOffset;
        tooltipPanel.transform.position = panelPos;
        string toShow = "";
        
        if (gos.isConsumer)
        {
            ConsumerScript consumerScript = gameObject.GetComponent<ConsumerScript>();
            //Debug.Log("Hovering over consumer!");
            if (isSmart)
            {
                toShow +=
                    $"Consuming {consumerScript.consumptionCurve[timeManager.hours]} power\n";
            }
            else
            {
                toShow +=
                    $"Consuming between {consumerScript.consumptionCurve.Max()} and {consumerScript.consumptionCurve.Min()} power\n";
            }
        }
        else if (gos.isGenerator)
        {
            toShow += $"Generating {gameObject.GetComponent<GeneratorScript>().amount} power\n";
            toShow += $"{storage.powerStored} power stored\n";
        }

        toShow += $"{gos.connections.Count + gos.consumerConnections.Count} / {gos.maxConnectiions} connections\n";

        //Debug.Log($"toShow: {toShow}");        

        powerAmtText.text = toShow;
    }
    

}
