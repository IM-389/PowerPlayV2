using System;
using UnityEngine;

namespace Power.V2
{
    public class ConsumerScript : MonoBehaviour
    {
        private PowerAmountInfo amountInfo;
        
        private NetworkScript ns;

        public AnimationCurve consumptionCurve;
        private float consumptionModifier;

        private TimeManager timeManager;

        private int previousTimestep;

        public int moneyGained;
        private MoneyManager moneyManager;

        public GameObject alert;
        public GameObject smartPowerAlert;

        private GeneralObjectScript gos;

        static float consumingHouses;
        static float totalHouses;

        bool isPowered = false;

        public float maxConsumption = -1;
        public float minConsumption = Int32.MaxValue;
        private void Start()
        {
            timeManager = GameObject.FindObjectOfType<TimeManager>();
            ns = gameObject.GetComponent<NetworkScript>();
            amountInfo = gameObject.GetComponent<PowerAmountInfo>();
            moneyManager = GameObject.FindObjectOfType<MoneyManager>();
            gos = gameObject.GetComponent<GeneralObjectScript>();
            alert = GameObject.FindWithTag("alert").transform.GetChild(0).gameObject;
            alert.SetActive(false);

            for (int i = 0; i < consumptionCurve.length; ++i)
            {
                float value = consumptionCurve.Evaluate(i);
                if (value > maxConsumption)
                {
                    maxConsumption = value;
                }

                if (value < minConsumption)
                {
                    minConsumption = value;
                }
            }
            totalHouses++;
            Debug.Log(totalHouses);
        }

        private void Update()
        {
            NetworkManager manager = ns.manager;

            consumptionModifier = consumptionCurve.Evaluate(timeManager.hours);

            if (timeManager.hours != previousTimestep)
            {
                // Remove the consumption from the manager prior to updating amount
                manager.powerConsumed -= amountInfo.amountConsumed;
                
                // Update the amount so if the object is removed the manager updates properly
                amountInfo.amountConsumed = consumptionModifier;

                // Apply the change to the manager
                manager.powerConsumed += amountInfo.amountConsumed;

                // TODO: Find a way to move this to the NetworkManager
                if (manager.hasEnoughPower)
                {
                    // Turns house powered on once when it has enough power
                    if (!isPowered)
                    {
                        consumingHouses++;
                        isPowered = true;
                        smartPowerAlert.SetActive(false);
                    }
                    moneyManager.money += moneyGained;
                }
                else if (gos.isSmart)
                {
                    if (isPowered)
                    {
                        smartPowerAlert.SetActive(true);
                        consumingHouses--;
                        isPowered = false;
                    }
                }
                else
                {
                    if (isPowered)
                    {
                        consumingHouses--;
                        isPowered = false;
                    }
                    smartPowerAlert.SetActive(false);
                }
                //Debug.Log(consumingHouses);
                // Turns on alert if house is not attached
                if(consumingHouses == totalHouses)
                {
                    alert.SetActive(false);
                }
                else
                {
                    alert.SetActive(true);
                }
                // City Approval stuff
                if (timeManager.hours == 25)
                {
                    if (!isPowered)
                    {
                        Debug.Log("City approval should go down at the end of the day");
                    }
                    else
                    {
                        if (consumingHouses / totalHouses >= 0.50)
                        {
                            timeManager.cityApproval += 10;
                        }
                        else if (consumingHouses / totalHouses >= 1)
                        {
                            timeManager.cityApproval += 25;
                        }
                    }
                    RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 3f, Vector2.zero);
                    foreach(RaycastHit2D hit in hits)
                    {
                        Debug.Log(hit.transform.gameObject);
                    }
                }
            }

            previousTimestep = timeManager.hours;

        }

        public NetworkManager GetManager()
        {
            return ns.manager;
        }
    }
}