using System.Collections;
using UnityEngine;

namespace Power.V2
{
    public class GeneratorScript : MonoBehaviour
    {
        public PowerManager.POWER_TYPES type;

        private PowerAmountInfo amountInfo;
        protected float basePower;
        
        /// <summary>
        /// Is this object immune to the power adjusts. 1 if no, 0 if yes
        /// </summary>
        protected bool powerAdjustImmune = false;

        private NetworkScript ns;

        private TimeManager timeManager;

        private int previousTimestep;

        public int upkeepCost;
        private MoneyManager moneyManager;
        private PowerManager powerManager;
        private void Start()
        {
            timeManager = GameObject.FindObjectOfType<TimeManager>();
            ns = gameObject.GetComponent<NetworkScript>();
            amountInfo = gameObject.GetComponent<PowerAmountInfo>();
            moneyManager = GameObject.FindObjectOfType<MoneyManager>();
            powerManager = GameObject.FindObjectOfType<PowerManager>();
            basePower = amountInfo.amountGenerated;
        }

        private void Update()
        {
            NetworkManager manager = ns.manager;
            

            if (timeManager.hours != previousTimestep)
            {
                // Remove the previously generated amount
                manager.powerGenerated -= amountInfo.amountGenerated;
                
                // Update the amount so if the object is removed the manager updates properly
                amountInfo.amountGenerated =
                    basePower * (powerAdjustImmune ? 1 : powerManager.powerAdjusts[(int) type]);

                // Apply the changes to the manager again
                manager.powerGenerated += amountInfo.amountGenerated;

                // TODO: Find a way to move this to the NetworkManager
                moneyManager.money -= upkeepCost;
            }

            previousTimestep = timeManager.hours;

        }

        public IEnumerator TimedDisable(float seconds)
        {
            NetworkManager manager = ns.manager;

            // Remove the previously generated amount
            manager.powerGenerated -= amountInfo.amountGenerated;
                
            // Update the amount so if the object is removed the manager updates properly
            amountInfo.amountGenerated = 0;

            // Apply the changes to the manager again
            //manager.powerGenerated += amountInfo.amountGenerated;

            yield return new WaitForSeconds(seconds);

            // Remove the previously generated amount
            manager.powerGenerated -= amountInfo.amountGenerated;
            
            amountInfo.amountGenerated = basePower;
            
            // Apply the changes to the manager again
            manager.powerGenerated += amountInfo.amountGenerated;
        }
        
        public virtual void DoUpgrade()
        {
            
        }
        
    }
    
}