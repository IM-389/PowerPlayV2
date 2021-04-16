using Power.V2;
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
    private PowerAmountInfo amountInfo;

    [Tooltip("Where to locate the tooltip relative to the object")]
    public Vector2 tooltipOffset;

    private PowerManager powerManager;
    private void Start()
    {
        tooltipPanel = GameObject.FindWithTag("TooltipPanel");
        powerAmtText = tooltipPanel.transform.GetChild(1).GetComponent<Text>();
        gos = gameObject.GetComponent<GeneralObjectScript>();
        amountInfo = gameObject.GetComponent<PowerAmountInfo>();
        GameObject gameController = GameObject.FindWithTag("GameController");
        timeManager = gameController.GetComponent<TimeManager>();
        powerManager = gameController.GetComponent<PowerManager>();
    }
    
    
    public void UpdateTooltip()
    {
        Vector2 panelPos = transform.position;
        panelPos += tooltipOffset;
        tooltipPanel.transform.position = panelPos;
        string toShow = "";

        toShow += amountInfo.gameObject.GetComponent<GeneralObjectScript>().buildingText + "\n";
        
        if (gos.isConsumer)
        {
            ConsumerScript consumerScript = gameObject.GetComponent<ConsumerScript>();
            //Debug.Log("Hovering over consumer!");
            if (gos.isSmart)
            {
                toShow +=
                    $"Consuming {consumerScript.consumptionCurve.Evaluate(timeManager.hours)} power\n";
            }
            else
            {
                toShow +=
                    $"Consumes between {consumerScript.minConsumption} and {consumerScript.maxConsumption} power\n";
            }
        }
        else if (gos.isGenerator)
        {
            toShow += $"Generating {amountInfo.amountGenerated} power\n";
        }

        toShow += $"{gos.nonConsumerConnections.Count} / {gos.maxConnections} connections\n";
        //toShow += $"{gos.lvConnections.Count} / {gos.maxLVConnections} LV connections\n";

        //Debug.Log($"toShow: {toShow}");        

        powerAmtText.text = toShow;
    }

    public void ToggleBuildCircle(bool show)
    {
        //Debug.Log($"Setting buildcircle on {gameObject.name}!");
        gos.buildCircle.SetActive(show);
    }


}
