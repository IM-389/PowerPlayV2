using Milestones;
using Power.V2;
using UnityEngine;

public class MilestoneSix : MilestoneBase
{
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
}
