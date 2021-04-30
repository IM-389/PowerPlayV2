using Milestones;
using Power.V2;
using UnityEngine;
using System.Collections;
public class PoweringIndustry : MilestoneBase
{
    [Tooltip("Area to upgrade the houses in")]
    public GameObject[] upgradeAreas;
    public override bool CheckCompleteMilestone()
    {
        GameObject[] allFactories = GameObject.FindGameObjectsWithTag("factory");
        GameObject[] allHospitals = GameObject.FindGameObjectsWithTag("hospital");

        int poweredFactories = 0;
        int poweredHospitals = 0;

        foreach (var factory in allFactories)
        {
            if (factory.GetComponent<ConsumerScript>().GetManager().hasEnoughPower)
            {
                ++poweredFactories;
            }
        }

        foreach (var hospital in allHospitals)
        {
            if (hospital.GetComponent<ConsumerScript>().GetManager().hasEnoughPower)
            {
                ++poweredHospitals;
            }
        }

        return (poweredFactories > 0 && poweredHospitals > 0);

    }
    
    public override void SetCompleteMilestone()
    {
        base.SetCompleteMilestone();

        foreach (var upgradeArea in upgradeAreas)
        {
            for (int i = 0; i < upgradeArea.transform.childCount; ++i)
            {
                if (upgradeArea.transform.GetChild(i).CompareTag("house"))
                {
                    upgradeArea.transform.GetChild(i).GetComponent<GeneralObjectScript>().isSmart = true;
                    upgradeArea.transform.GetChild(i).GetChild(5).gameObject.SetActive(true);
                }
                
            }

        }
        StartCoroutine(AutoUpgrade());
    }
    private IEnumerator AutoUpgrade()
    {
        WaitForSeconds wait = new WaitForSeconds(0.5f);
        while (true)
        {
            GameObject[] allSubstations = GameObject.FindGameObjectsWithTag("Substation");


            foreach (var substation in allSubstations)
            {

                if (!substation.GetComponent<GeneralObjectScript>().isSmart)
                {
                    substation.GetComponent<GeneralObjectScript>().isSmart = true;
                }
            }
            yield return wait;
        }
    }
}
