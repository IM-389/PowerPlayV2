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
        
        public GameObject smartPowerAlert;

        private GeneralObjectScript gos;
        private void Start()
        {
            timeManager = GameObject.FindObjectOfType<TimeManager>();
            ns = gameObject.GetComponent<NetworkScript>();
            amountInfo = gameObject.GetComponent<PowerAmountInfo>();
            moneyManager = GameObject.FindObjectOfType<MoneyManager>();
            gos = gameObject.GetComponent<GeneralObjectScript>();
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
                    moneyManager.money += moneyGained;
                    smartPowerAlert.SetActive(false);
                }
                else if (gos.isSmart)
                {
                    smartPowerAlert.SetActive(true);
                }
                else
                {
                    smartPowerAlert.SetActive(false);
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