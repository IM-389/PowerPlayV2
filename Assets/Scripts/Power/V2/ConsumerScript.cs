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
        static bool check = false;
        public static bool approvalTracking;

        bool isPowered = false;

        // House: 1 Hospital: 20 Factory: 10
        public int approvalIncrease;
        // House: 3 Hospital: 35 Factory: 15
        public int approvalDecrease;

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
                if (timeManager.hours == 1)
                {
                    check = false;
                }
                if (timeManager.hours == 25 && approvalTracking)
                {
                    if (!isPowered)
                    {
                        timeManager.DecreaseCitySat(approvalDecrease);
                    }
                    else
                    {
                        timeManager.IncreaseCitySat(approvalIncrease);
                    }
                    if(!check && consumingHouses/totalHouses >= 1)
                    {
                        timeManager.IncreaseCitySat(25);
                        Debug.Log("Congrats, all of your consumers are powered");
                        check = true;
                    }
                    else if(!check && consumingHouses / totalHouses >= 0.5)
                    {
                        timeManager.IncreaseCitySat(10);
                        Debug.Log("Congrats, half or more of your consumers are powered");
                        check = true;
                    }
                }
            }

            previousTimestep = timeManager.hours;

        }
        public void TrackApproval()
        {
            approvalTracking = true;
        }
        public NetworkManager GetManager()
        {
            return ns.manager;
        }
    }
}