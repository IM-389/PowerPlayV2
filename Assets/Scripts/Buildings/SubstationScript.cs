using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Power.V2;
public class SubstationScript : MonoBehaviour
{

   
    GeneralObjectScript gos;
    NetworkScript power;
    void Start()
    {
        gos = gameObject.GetComponent<GeneralObjectScript>();
        power = gameObject.GetComponent<NetworkScript>();
    }
   
    void Update()
    {
        
        //iftotalpower - coal power > powerconsumed, tunr off coal gen by either
        //setting powergenerated on em to 0  or mask it
        if(gos.isSmart)
        {
            GameObject[] generators = GameObject.FindGameObjectsWithTag("Generator");
            float coalPower = 0;
            float nonCoalPower = 0;
            List<GameObject> coal = new List<GameObject>();
            //gets the amounts of power
            for (int i = 0; i < generators.Length; i++)
            {
                if (generators[i].GetComponent<NetworkScript>().manager == power.manager)
                {
                   if(generators[i].GetComponent<GeneratorScript>().type == PowerManager.POWER_TYPES.TYPE_COAL)
                    {
                       coalPower += generators[i].GetComponent<PowerAmountInfo>().amountGenerated;
                       coal.Add(generators[i]);
                    }
                    else
                    {
                        nonCoalPower += generators[i].GetComponent<PowerAmountInfo>().amountGenerated;
                    }
                }
            }
            if(power.manager.powerConsumed < nonCoalPower)
            {
                //turn off coal gens 
                for(int i = 0; i < coal.Count; i++)
                {
                    //spare us designers
                    coal[i].GetComponent<PowerAmountInfo>().amountGenerated = 0;
                }
            }
            else
            {
                for (int i = 0; i < coal.Count; i++)
                {
                    //spare us designers
                    coal[i].GetComponent<PowerAmountInfo>().amountGenerated = 30;
                }
            }
        }
       
    }
    
   
}
