using System.Collections;
using System.Collections.Generic;
using Power.V2;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    
    [Tooltip("Text for coal, gas, solar, and wind respectivly")]
    public Text[] powerAmountText;

    [Tooltip("Text for the total power amount")]
    public Text totalPowerText;

    [Tooltip("Text for the amount of money the player currently has")]
    public Text moneyText;

    private PowerManager powerManager;

    private MoneyManager moneyManager;
    

    [Tooltip("What to show when a house is out of power")]
    public GameObject outOfPowerAlert;
    // Start is called before the first frame update
    void Start()
    {
        powerManager = gameObject.GetComponent<PowerManager>();
        moneyManager = gameObject.GetComponent<MoneyManager>();
        StartCoroutine(UpdateUI());
    }

    private IEnumerator UpdateUI()
    {
        while (true)
        {
            float totalGenerated = 0;
            for (int i = 0; i < 4; ++i)
            {
                //Debug.Log((PowerManager.POWER_TYPES)i);
                powerManager.CalculateAmountsGenerated((PowerManager.POWER_TYPES)i);
            }

            for (int i = 0; i < powerManager.powerAmountsGenerated.Length; ++i)
            {
                totalGenerated += powerManager.powerAmountsGenerated[i];
                powerAmountText[i].text = powerManager.powerAmountsGenerated[i].ToString();
            }

            totalPowerText.text = totalGenerated.ToString();

            moneyText.text = "$" + moneyManager.money.ToString();

            NetworkManager[] managers = GameObject.FindObjectsOfType<NetworkManager>();
            int numOutOfPower = 0;
            foreach (var manager in managers)
            {
                if (!manager.hasEnoughPower)
                {
                    ++numOutOfPower;
                }
            }

            if (numOutOfPower > 0)
            {
                outOfPowerAlert.SetActive(true);
            }
            else
            {
                outOfPowerAlert.SetActive(false);
            }
            
            yield return new WaitForSecondsRealtime(0.5f);
        }
    }
}
