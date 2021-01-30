using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class Phase2Manager : MonoBehaviour
{
    public GameObject coalReference;
    public CoalScript coal;
    public GameObject turbineReference;
    public TurbineScript turbine;
    public GameObject gasReference;
    public NaturalGasScript gas;
    public GameObject solarReference;
    public SolarScript solar;

    [Tooltip("The amount of happiness the population has")]
    public float happiness;
    [Tooltip("The levels of pollution currently existing")]
    public float pollutionLevels;
    [Tooltip("The multiples of pollution where if pollution beats it, will cause happiness to drop")]
    public float pollutionTollerance;
    [Tooltip("The amount of times over pollution level beats pollution Tollerance.  Used to track negative events related to polution")]
    public int pollutionTicks;
    [Tooltip("The starting amount of currency the player has")]
    public int startingCurrency;
    [Tooltip("The amount of currency the player has")]
    public static float currency;
    [Tooltip("The amount of currency from the previous simulation step")]
    public float previousCurrency;
    [Tooltip("The current population")]
    public float population;
    [Tooltip("The amount the player earns at the end of each turn per population amount")]
    public float currencyPerPerson;

    [Tooltip("The amount of power needed to increase happiness")]
    public float powerNeeded;
    [Tooltip("The currentPower")]
    public float currentPower;
    [Tooltip("The amount of excess power needed to grow the population")]
    [Range(1, 100)]
    public float excessPowerGrowth;

    [Tooltip("The amount of power needed for every house")]
    public float powerNeededPerPerson = 1f;

    [Header("Random Event Information and UI Display")]
    public float coalMultiplier = 1f;
    public float gasMultiplier = 1f;
    public float windMultiplier = 1f;
    public float solarMultiplier = 1f;
    public float coalPercentage;
    public float coalTotal;
    public float gasPercentage;
    public float gasTotal;
    public float windPercentage;
    public float windTotal;
    public float solarPercentage;
    public float solarTotal;
    public float previousPopulation;
    public float previousPollution;

    public TextMeshProUGUI currencyAmount;
    public TextMeshProUGUI populationAmount;
    public TextMeshProUGUI environmentThing;
    public TextMeshProUGUI totalPower;
    public TextMeshProUGUI gasPower;
    public TextMeshProUGUI gasPowerPercentage;
    public TextMeshProUGUI coalPower;
    public TextMeshProUGUI coalPowerPercentage;
    public TextMeshProUGUI windPower;
    public TextMeshProUGUI windPowerPercentage;
    public TextMeshProUGUI solarPower;
    public TextMeshProUGUI solarPowerPercentage;
    public TextMeshProUGUI previousPollutionText;
    public TextMeshProUGUI previousPopulationText;
    public TextMeshProUGUI housesPoweredText;
    public TextMeshProUGUI housesUnpoweredText;

    //summary screen components
    public TextMeshProUGUI summaryGas;
    public TextMeshProUGUI summaryCoal;
    public TextMeshProUGUI summarySolar;
    public TextMeshProUGUI summaryTurbine;
    public TextMeshProUGUI summarySpent;
    public TextMeshProUGUI summaryGained;
    public TextMeshProUGUI summaryPreviousEnviroment;
    public TextMeshProUGUI summaryPresentEnviroment;
    public TextMeshProUGUI summaryPreviousPopulation;
    public TextMeshProUGUI summaryPresentPopulation;
    public TextMeshProUGUI summaryRandomEvent;

    public int day = 0;
    public GameObject simulationSummaryPanel;

    private const int maxCameraSize = 13;

    public enum allRandomEvents { smog, treesFall, unhealthyAir, windmillBreaks, protests, gasLeak, cloudyDay };
    public allRandomEvents currentEvent;

    public float smogSolarEffect = .9f;
    public float smogThreshold = 50f;
    public float unhealthyAirThreshold = 25f;
    public float unhealthyAirMultiplier = 0f;
    public float cloudsMultiplier = .5f;
    StartUpScript start;

    BuildFunctions build;

    public static int amountOfHousesPowered = 0;
    public static int amountOfHousesUnpowered = 0;

    [Range(0, 1)]
    public float solarDeviationMin;
    [Range(0, 1)]
    public float solarDeviationMax;
    [Range(0, 1)]
    public float windDeviationMin;
    [Range(0, 1)]
    public float windDeviationMax;

    public GameObject simulationButton;
    public GameObject eventPanel;
    public TextMeshProUGUI eventPanelTitle;
    public TextMeshProUGUI eventPanelText;

    private void Start()
    {
        currency = startingCurrency;

        build = FindObjectOfType<BuildFunctions>();
        coal = coalReference.GetComponent<CoalScript>();
        turbine = turbineReference.GetComponent<TurbineScript>();
        gas = gasReference.GetComponent<NaturalGasScript>();
        solar = solarReference.GetComponent<SolarScript>();



        TriggerEvent();
        if (excessPowerGrowth < 1)
        {
            excessPowerGrowth = 1;
        }



    }
    /*
     * Powered Houses
     * Unpowered Houses
     * Start Random Events
     */

    public void TriggerEvent()
    {
        start = FindObjectOfType<StartUpScript>();


        RunSimulation(BuildFunctions.coalAmount, BuildFunctions.turbineAmount, BuildFunctions.gasAmount, BuildFunctions.solarAmount, StartUpScript.houseAmount);

        //runs some stuff depending on what day it is
        switch (day)
        {
            case 0:
                BuildFunctions.simulationReset = false;
                amountOfHousesUnpowered = (int)population;
                amountOfHousesPowered = 0;
                break;

            default:
                simulationSummaryPanel.SetActive(true);
                simulationButton.SetActive(false);

                AdjustPopulation();
                break;
        }

        //incriment day
        day++;

        //resets the house count amount
        BuildFunctions.simulationReset = false;
        amountOfHousesUnpowered = (int)population;
        amountOfHousesPowered = 0;
        progressEvents();
    }

    //updates the total power ui element
    public void UpdateUi(int coalAmount, int turbineAmount, int gasAmount, int solarAmount)
    {
        currentPower = 0;
        coalTotal = coalAmount * coal.power * coalMultiplier;

        /*
        //wind values calculated
        windTotal = 0;
        var objects = GameObject.FindGameObjectsWithTag("turbine");
        var objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<TurbineScript>().broken == false)
            {
                windTotal += objects[i].GetComponent<TurbineScript>().power * windMultiplier;
            }
        }

        //wind values calculated
        gasTotal = 0;
        objects = GameObject.FindGameObjectsWithTag("gasPlant");
        objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<NaturalGasScript>().broken == false)
            {
                gasTotal += objects[i].GetComponent<NaturalGasScript>().power * gasMultiplier;
            }
        }

        //solar values calculated
        solarTotal = 0;
        objects = GameObject.FindGameObjectsWithTag("solar");
        objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            solarTotal += objects[i].GetComponent<SolarScript>().power * solarMultiplier;
        }
        */
        windTotal = turbineAmount * turbine.power * windMultiplier;
        gasTotal = gasAmount * gas.power * gasMultiplier;
        solarTotal = solarAmount * solar.power * solarMultiplier;
        currentPower = coalTotal + windTotal + solarTotal + gasTotal;
        totalPower.text = currentPower.ToString();

        solarPower.text = solarTotal.ToString();
        coalPower.text = coalTotal.ToString();
        windPower.text = windTotal.ToString();
        gasPower.text = gasTotal.ToString();


        coalPercentage = coalTotal / currentPower;
        windPercentage = windTotal / currentPower;
        solarPercentage = solarTotal / currentPower;
        gasPercentage = gasTotal / currentPower;

        populationAmount.text = StartUpScript.houseAmount.ToString();

        if (currentPower != 0)
        {
            solarPowerPercentage.text = System.String.Format("{0:0.0%}", solarPercentage);
            coalPowerPercentage.text = System.String.Format("{0:0.0%}", coalPercentage);
            windPowerPercentage.text = System.String.Format("{0:0.0%}", windPercentage);
            gasPowerPercentage.text = System.String.Format("{0:0.0%}", gasPercentage);
        }
        else
        {
            solarPowerPercentage.text = "0.0%";
            coalPowerPercentage.text = "0.0%";
            windPowerPercentage.text = "0.0%";
            gasPowerPercentage.text = "0.0%";
        }

        housesPoweredText.text = amountOfHousesPowered.ToString();
        housesUnpoweredText.text = amountOfHousesUnpowered.ToString();
    }

    //updates the total currency amount
    public void UpdateCurrency()
    {
        currencyAmount.text = currency.ToString();
    }



    public void RunSimulation(int coalAmount, int turbineAmount, int gasAmount, int solarAmount, int houseAmount)
    {

        if (brokeWindmill)
        {
            turbineAmount -= 1;
            brokeWindmill = false;
        }
        if (gasLeak)
        {
            turnCount++;
            if (turnCount <= 2)
            {
                if (gasAmount >= 1)
                {
                    gasAmount--;
                }
            }
            else
            {
                turnCount = 0;
                gasLeak = false;
            }
        }
        if (protest)
        {
            if (gasAmount >= 1)
            {
                gasAmount--;
            }
        }

        summarySpent.text = (Mathf.Abs(currency - previousCurrency)).ToString();
        previousPopulation = population;
        previousPollution = pollutionLevels;
        previousCurrency = currency;
        //previousPollutionText.text = previousPollution.ToString();
        //int unpoweredHouses = houseAmount - poweredHouses;


        summaryPreviousPopulation.text = previousPopulation.ToString();



        summaryPreviousEnviroment.text = previousPollution.ToString();

        powerNeeded = population * powerNeededPerPerson;
        pollutionLevels -= coal.pollution * coalAmount;
        pollutionLevels -= gas.pollution * gasAmount;

        summaryPresentEnviroment.text = pollutionLevels.ToString();

        currentPower = 0;
        coalTotal = coalAmount * coal.power * coalMultiplier;
        /*
        //wind values calculated
        windTotal = 0;
        var objects = GameObject.FindGameObjectsWithTag("turbine");
        var objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<TurbineScript>().broken == false)
            {
                windTotal += objects[i].GetComponent<TurbineScript>().power * windMultiplier;
            }
        }

        //gasvalues calculated
        gasTotal = 0;
        objects = GameObject.FindGameObjectsWithTag("gasPlant");
        objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<NaturalGasScript>().broken == false)
            {
                gasTotal += objects[i].GetComponent<NaturalGasScript>().power * gasMultiplier;
            }
        }

        //solar values calculated
        solarTotal = 0;
        objects = GameObject.FindGameObjectsWithTag("solar");
        objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
                solarTotal += objects[i].GetComponent<SolarScript>().power * solarMultiplier;
        }
        */

        windTotal = turbineAmount * turbine.power * windMultiplier;
        gasTotal = gasAmount * gas.power * gasMultiplier;
        solarTotal = solarAmount * solar.power * solarMultiplier;
        currentPower = coalTotal + windTotal + solarTotal + gasTotal;
        coalPercentage = coalTotal / currentPower;
        windPercentage = windTotal / currentPower;
        solarPercentage = solarTotal / currentPower;
        gasPercentage = gasTotal / currentPower;


        solarPower.text = solarTotal.ToString();
        coalPower.text = coalTotal.ToString();
        windPower.text = windTotal.ToString();
        gasPower.text = gasTotal.ToString();

        totalPower.text = currentPower.ToString();
        /*
        totalPower.text = currentPower.ToString();
        totalPower.text = currentPower.ToString();
        */

        solarPowerPercentage.text = System.String.Format("{0:0.0%}", solarPercentage);
        coalPowerPercentage.text = System.String.Format("{0:0.0%}", coalPercentage);
        windPowerPercentage.text = System.String.Format("{0:0.0%}", windPercentage);
        gasPowerPercentage.text = System.String.Format("{0:0.0%}", gasPercentage);


        if (pollutionLevels > pollutionTollerance * pollutionTicks)
        {
            pollutionTicks++;
        }

        /*
        if (amountOfHousesUnpowered <= 0)
        {
            while (currentPower > powerNeeded)
            {

                happiness++;
                currentPower -= powerNeeded;
            }
        }



        if (happiness >= population)
        {
            population = happiness;
        }

        //happiness is a variable that makes me unhappy
        */


        //population increases if all houses powered
        if (amountOfHousesUnpowered <= 0 && day > 0)
        {

            population += (int)amountOfHousesPowered / 2;


            if (GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize < maxCameraSize)
            {
                GameObject.Find("MainCamera").GetComponent<Camera>().orthographicSize += .5f;
            }

        }

        //population decereases when houses unpowered but 1 unpowered house will always remain
        if (amountOfHousesUnpowered > 0 && day > 0)
        {
            int count = amountOfHousesUnpowered;
            if (amountOfHousesUnpowered > 1)
            {
                while (count > 1)
                {
                    population--;
                    count--;

                }
            }

        }


        /*
        if (amountOfHousesUnpowered > 3)
        {
            int totalUnpowered = amountOfHousesUnpowered;

            while (totalUnpowered > 2)
            {
                population--;
                totalUnpowered -= 2;
            }



        }
        */


        housesPoweredText.text = amountOfHousesPowered.ToString();
        housesUnpoweredText.text = amountOfHousesUnpowered.ToString();


        //Something involving punishments with pollutionTicks.

        currency += amountOfHousesPowered * currencyPerPerson;
        //currency -= coal.coalUpkeep + gasAmount.
        currency -= CoalScript.upkeep * coalAmount;
        currency -= NaturalGasScript.upkeep * gasAmount;
        currency -= SolarScript.upkeep * solarAmount;
        currency -= TurbineScript.upkeep * turbineAmount;

        currencyAmount.text = currency.ToString();
        populationAmount.text = StartUpScript.houseAmount.ToString();
        environmentThing.text = pollutionLevels.ToString();
        UpdateUi(coalAmount, turbineAmount, gasAmount, solarAmount);

        coalMultiplier = 1f;
        gasMultiplier = 1f;
        solarMultiplier = Random.Range(solarDeviationMin, solarDeviationMax);
        windMultiplier = Random.Range(windDeviationMin, windDeviationMax);
        RollRandom();

        summaryCoal.text = coalTotal.ToString();
        summarySolar.text = solarTotal.ToString();
        summaryTurbine.text = windTotal.ToString();
        summaryGas.text = gasTotal.ToString();


        summaryPresentPopulation.text = population.ToString(); ;


        summaryGained.text = (currency - previousCurrency).ToString();



        summaryRandomEvent.text = currentEvent.ToString();

    }

    public void RollRandom()
    {
        int RandomValue;
        int totalRandom = 5;

        if (pollutionLevels < smogThreshold)
        {
            totalRandom++;
        }
        if (pollutionLevels < unhealthyAirThreshold)
        {
            totalRandom++;
        }

        RandomValue = Random.Range(0, totalRandom);

        if (RandomValue == 1)
        {
            if (GameObject.Find("turbine(Clone)") == null)
            {
                RandomValue = 0;
            }
        }
        else if (RandomValue == 2 || RandomValue == 3)
        {
            if(GameObject.Find("naturalgasplant(Clone)") == null)
            {
                RandomValue = 4;
            }
        }

        switch (RandomValue)
        {
            case 0:
                TreesFall();
                eventPanelText.text = "A tree has fallen within your city and possibly destroyed a powerpole.";
                break;
            case 1:
                WindmillBreak();
                eventPanelText.text = "A wind turbine has broken down." +
                    "\n\n" +
                    "It will not produce energy for one turn and needs to be repaired.";
                break;
            case 2:
                GasLeak();
                eventPanelText.text = "A gas leak has occurred at the natural gas plant" +
                    "\n\n" +
                    "The natural gas plant will be closed for two turns and need to be repaired";
                break;
            case 3:
                Protest();
                eventPanelText.text = "Protests have occurred due to a natural gas fire that killed many birds." +
                    "\n\n" +
                    "The natural gas plant will close for one turn and you will be fined for its cost.";
                break;
            case 4:
                Clouds();
                eventPanelText.text = "There is no sun out today." +
                    "\n\n" +
                  "Solar panels will produce half as much power.";
                break;
            case 5:
                SmogEvent();
                eventPanelText.text = "Pollution has reached an unhealthy level." +
                    "\n\n" +
                    "Solar panels will generate less power for one turn.";
                break;
            case 6:
                UnheathlyAir();
                eventPanelText.text = "Pollution has reached an unhealthy level." +
                    "\n\n" +
                    "Coal and natural gas plants won't produce energy for one turn.";
                break;
        }
        eventPanelTitle.text = currentEvent.ToString();

    }

    public void SmogEvent()
    {
        currentEvent = allRandomEvents.smog;
        solarMultiplier = smogSolarEffect;
        var objects = GameObject.FindGameObjectsWithTag("solar");
        var objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            objects[i].GetComponent<SolarScript>().daysToBeBroken = 2;
        }
    }

    public void TreesFall()
    {
        GameObject[] allPower = GameObject.FindGameObjectsWithTag("Power");
        List<GameObject> nextToTrees = new List<GameObject>();
        for (int i = 0; i < allPower.Length; i++)
        {
            if (build.isObjectNextToTree(allPower[i]))
            {
                nextToTrees.Add(allPower[i]);
            }
        }

        if (nextToTrees == null)
        {
            return;
        }

        int rollRandom = Random.Range(0, nextToTrees.Count);
        GameObject[] toArray = nextToTrees.ToArray();

        //Following function removes gameobject next to tree //previously this was the destroy function
        if(rollRandom < nextToTrees.Count && toArray[rollRandom] != null)
        {
            GameObject.Find("GameManager").GetComponent<BuildFunctions>().RemoveObjectFunction(toArray[rollRandom]);
        }


        currentEvent = allRandomEvents.treesFall;

    }

    public void UnheathlyAir()
    {
        coalMultiplier = unhealthyAirMultiplier;
        gasMultiplier = unhealthyAirMultiplier;
        currentEvent = allRandomEvents.unhealthyAir;
        var objects = GameObject.FindGameObjectsWithTag("coal");
        var objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            objects[i].GetComponent<CoalScript>().daysToBeBroken = 2;
        }

        objects = GameObject.FindGameObjectsWithTag("gasPlant");
        objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            objects[i].GetComponent<NaturalGasScript>().daysToBeBroken = 2;//days needs to have an extra for reasons
        }
    }
    bool brokeWindmill;
    public void WindmillBreak()
    {
        brokeWindmill = true;
        currentEvent = allRandomEvents.windmillBreaks;
        var objects = GameObject.FindGameObjectsWithTag("turbine");
        var objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<TurbineScript>().broken == false)
            {
                objects[i].GetComponent<TurbineScript>().broken = true;
                objects[i].GetComponent<TurbineScript>().daysToBeBroken = 2;//days needs to have an extra for reasons
                break;
            }
        }
    }

    bool gasLeak;
    int turnCount;
    public void GasLeak()
    {
        gasLeak = true;
        currentEvent = allRandomEvents.gasLeak;
        var objects = GameObject.FindGameObjectsWithTag("gasPlant");
        var objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<NaturalGasScript>().broken == false)
            {
                objects[i].GetComponent<NaturalGasScript>().broken = true;
                objects[i].GetComponent<NaturalGasScript>().daysToBeBroken = 3;//days needs to have an extra for reasons
                break;
            }
        }
    }

    bool protest;
    public void Protest()
    {
        protest = true;
        currency -= NaturalGasScript.cost;
        currentEvent = allRandomEvents.protests;
        var objects = GameObject.FindGameObjectsWithTag("gasPlant");
        var objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<NaturalGasScript>().broken == false)
            {
                objects[i].GetComponent<NaturalGasScript>().daysToBeBroken = 2;//days needs to have an extra for reasons
                currency -= NaturalGasScript.cost;
                break;
            }
        }
    }

    public void Clouds()
    {
        solarMultiplier = cloudsMultiplier;
        currentEvent = allRandomEvents.cloudyDay;
        var objects = GameObject.FindGameObjectsWithTag("solar");
        var objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            objects[i].GetComponent<SolarScript>().daysToBeBroken = 2;
        }
    }

    public void AdjustPopulation()
    {


        if ((int)population < StartUpScript.houseAmount)
        {
            //amount of houses to remove
            int housesToRemove = (StartUpScript.houseAmount - (int)population);

            //removes houses
            GameObject.Find("GameManager").GetComponent<StartUpScript>().removeHouses(housesToRemove);

        }
        else if ((int)population > StartUpScript.houseAmount)
        {
            //amount of houses to add
            int housesToAdd = (int)population - StartUpScript.houseAmount;

            //adds houses
            GameObject.Find("GameManager").GetComponent<StartUpScript>().spawnMoreHouses(housesToAdd);
        }
    }

    public void showEventPanel()
    {
        simulationSummaryPanel.SetActive(false);
        simulationButton.SetActive(false);
        eventPanel.SetActive(true);
    }

    public void exitEventPanel()
    {


        simulationSummaryPanel.SetActive(false);
        simulationButton.SetActive(true);
        eventPanel.SetActive(false);

    }

    public void progressEvents()
    {
        var objects = GameObject.FindGameObjectsWithTag("turbine");
        var objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<TurbineScript>().daysToBeBroken > 0)
            {
                objects[i].GetComponent<TurbineScript>().daysToBeBroken--;
            }
        }

        objects = GameObject.FindGameObjectsWithTag("gasPlant");
        objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<NaturalGasScript>().daysToBeBroken > 0)
            {
                objects[i].GetComponent<NaturalGasScript>().daysToBeBroken--;
            }
        }

        objects = GameObject.FindGameObjectsWithTag("solar");
        objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<SolarScript>().daysToBeBroken > 0)
            {
                objects[i].GetComponent<SolarScript>().daysToBeBroken--;
            }
        }

        objects = GameObject.FindGameObjectsWithTag("coal");
        objectCount = objects.Length;
        for (int i = 0; i < objectCount; i++)
        {
            if (objects[i].GetComponent<CoalScript>().daysToBeBroken > 0)
            {
                objects[i].GetComponent<CoalScript>().daysToBeBroken--;
            }
        }
    }
}
