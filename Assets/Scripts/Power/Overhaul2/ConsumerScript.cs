using UnityEngine;

namespace Power.Overhaul2
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
        
        private void Start()
        {
            timeManager = GameObject.FindObjectOfType<TimeManager>();
            ns = gameObject.GetComponent<NetworkScript>();
            amountInfo = gameObject.GetComponent<PowerAmountInfo>();
            moneyManager = GameObject.FindObjectOfType<MoneyManager>();
        }

        private void Update()
        {
            NetworkManager manager = ns.manager;

            consumptionModifier = consumptionCurve.Evaluate(timeManager.hours);

            if (timeManager.hours != previousTimestep)
            {
                // Apply the modifier to the amount, as the base should have been done when connected
                manager.powerConsumed += consumptionModifier;
                // Update the amount so if the object is removed the manager updates properly
                amountInfo.amountConsumed += consumptionModifier;

                // TODO: Find a way to move this to the NetworkManager
                if (manager.hasEnoughPower)
                {
                    moneyManager.money += moneyGained;
                }
            }

            previousTimestep = timeManager.hours;

        }
        
    }
}