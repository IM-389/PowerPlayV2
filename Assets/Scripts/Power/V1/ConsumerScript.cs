using System.Collections;
using UnityEngine;

namespace Power.V1
{

    public class ConsumerScript : PowerBase
    {

        [Tooltip("Is the object currently consuming power. True is yes, false if no")]
        public bool isConsuming;

        [Tooltip("Is the object currently gaining power. True if yes, false if no")]
        public bool isPowered;

        public GameObject alert;
        public GameObject alertArrow;

        public static int consumerOut;

        //public static int housesPowered;
        public bool powerOut = false;
        public bool powerOn = false;

        HoverScript hover;

        [Tooltip("How the consumption changes over time. Index is hour, value is consumption at that hour")]
        public float[] consumptionCurve = new float[24];

        public AnimationCurve consumptionCurve2;
        public GameObject buildingType;

        public float numHousesPowered = 0;

        public float
            totalHouses =
                0; //this may be wrong. I plan on incrementing it when the player connects to houses, but i will NOT decrement it
        //if they lose power. that way i can calculate the % of houses powered. 

        /// <summary>
        /// How much power to consume, adjusted from the curve
        /// </summary>
        private float consumeAmount;

        private float consumeAmount2;

        /// <summary>
        /// If the object is cut off from power. Set to true the tick the object does not have enough power
        /// Used to delay setting the isConsuming flag to give leeway with short power drops
        /// </summary>
        private bool isCutOff = false;

        private MoneyManager moneymanager;
        public TimeManager cityApproval;
        public int moneyGained;

        void Start()
        {
            base.Start();
            alert = GameObject.FindGameObjectWithTag("alert").transform.GetChild(0).gameObject;
            hover = GetComponent<HoverScript>();
            moneymanager = GameObject.FindWithTag("GameController").GetComponent<MoneyManager>();
            alert.SetActive(false);
            cityApproval = GameObject.FindGameObjectWithTag("GameController").GetComponent<TimeManager>();
        }

        // TODO: Fully implement
        protected override void Tick()
        {

            // TODO: Smooth the transition between values
            //consumeAmount = consumptionCurve[timeManager.hours];
            consumeAmount = consumptionCurve2.Evaluate(timeManager.hours);

            //Debug.Log(timeManager.hours);
            //Debug.Log("Consume Amount 1:" + consumeAmount);
            //Debug.Log("Consume Amount 2:" + consumeAmount2);
            // If there is enough power, consume it
            numHousesPowered = 0;
            GameObject[] houses = GameObject.FindGameObjectsWithTag("house");
            foreach (GameObject house in houses)
            {
                if (house.GetComponent<ConsumerScript>().isConsuming)
                {
                    numHousesPowered++;
                }
            }

            totalHouses = GameObject.FindGameObjectsWithTag("house").Length;
            if ((storageScript.powerStored >= consumeAmount))
            {
                storageScript.PullPower(consumeAmount);
                // Set the flag so other objects can know if this one is consuming
                isConsuming = true;
                isCutOff = false;
                moneymanager.money += moneyGained;
                //if (buildingType.CompareTag("house"))
                //{
                //    numHousesPowered++;
                //    totalHouses++;
                //}
            }

            else
            {
                // Set the flag so other objects can know if this one is consuming
                /*
                 * if (buildingType.CompareTag("house"))
                {
                    numHousesPowered--;
                }
                */
                //isCutOff = !isConsuming;
                isConsuming = false;
            }

            Debug.Log(cityApproval);
            if (cityApproval != null)
            {
                Debug.Log("This");
                if (cityApproval.hours == 25)
                {
                    Debug.Log("This2");
                    if (!isConsuming)
                    {
                        //calc % of houses powered
                        //if (buildingType.compareTag("house")/hospital)
                        //{

                        //     cityApproval.cityApproval -= 20;//we're gonna hope this works

                        // }
                        //if(buildingType.compareTag("hospital"))
                        // {
                        //     cityApproval.cityApproval -= 35;
                        // }
                        Debug.Log("City approval should go down at the end of the day");
                    }
                    else
                    {
                        if (numHousesPowered / totalHouses >= 0.50)
                        {
                            cityApproval.cityApproval += 10;
                        }
                        else if (numHousesPowered / totalHouses >= 1)
                        {
                            cityApproval.cityApproval += 25;
                        }
                    }

                }
            }
            //Debug.Log(gos.connected && !isConsuming);

            // Adds to Power out if the consumer is connected and not consuming power
            if (gos.connected && !isConsuming)
            {
                if (!powerOut)
                {
                    //Debug.Log("This is happening");
                    consumerOut++;
                    powerOut = true;

                }
            }
            else
            {
                if (powerOut)
                {
                    consumerOut--;
                    powerOut = false;
                }
            }

            // Tracks if a house is being powered on or not
            if (isConsuming)
            {
                if (!powerOn)
                {
                    //housesPowered++;
                    powerOn = true;
                }
            }
            else
            {
                if (powerOn)
                {
                    //housesPowered--;
                    powerOn = false;
                }
            }

            if (hover.GetComponent<GeneralObjectScript>().isSmart)
            {
                alertArrow.SetActive(powerOut);
            }

            if (consumerOut > 0)
            {
                alert.SetActive(true);
            }
            else
            {
                alert.SetActive(false);
            }
            //Debug.Log(consumerOut);
            //Debug.Log(housesPowered);


            if (gos.GetAllConnectionsCount() == 0)
            {
                storageScript.powerStored = 0;
            }
        }
    }
}
