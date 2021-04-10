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

        static float totalGenerators;
        static float coalGenerators;
        static bool check = false;

        public int upkeepCost;
        private MoneyManager moneyManager;
        private PowerManager powerManager;
        private void Start()
        {
            totalGenerators++;
            if(type == PowerManager.POWER_TYPES.TYPE_COAL)
            {
                coalGenerators++;
            }
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

                if(timeManager.hours == 1)
                {
                    check = false;
                }

                // City Approval Stuff
                if (timeManager.hours == 25)
                {
                    // If the generator is a coal plant or windmill
                    if(type == PowerManager.POWER_TYPES.TYPE_COAL || type == PowerManager.POWER_TYPES.TYPE_WIND)
                    {
                        // Make a circlecast of 7 squares
                        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, 6.5f, Vector2.zero);
                        foreach (RaycastHit2D hit in hits)
                        {
                            GameObject hitObject = hit.transform.gameObject;
                            // If one of the objects hit is a consumer, lower the city satisfaction.
                            if(hitObject.CompareTag("house") || hitObject.CompareTag("factory")|| hitObject.CompareTag("hospital"))
                            {
                                timeManager.cityApproval -= 10;
                                Debug.Log("Lower City satisfaction, coal plant/windmill is too close to house");
                            }
                        }
                        if (!check)
                        {
                            if(coalGenerators/totalGenerators >= 1f)
                            {
                                Debug.Log("100% of your generators are coal, try using something a little MMMM cleaner!");
                                timeManager.cityApproval -= 40;
                            }
                            else if(coalGenerators/totalGenerators >= 0.9f)
                            {
                                Debug.Log("90% of your generators are coal, try using something a little MMMM cleaner!");
                                timeManager.cityApproval -= 20;
                            }
                            else if (coalGenerators/totalGenerators >= 0.8f)
                            {
                                Debug.Log("80% of your generators are coal, try using something a little MMMM cleaner!");
                                timeManager.cityApproval -= 10;
                            }
                            check = true;
                        }
                    }
                }
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