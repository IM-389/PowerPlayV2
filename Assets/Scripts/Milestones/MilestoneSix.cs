using Milestones;
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
            if (factory.GetComponent<StorageScript>().powerStored > 0)
            {
                ++poweredFactories;
            }
        }

        foreach (var hospital in allHospitals)
        {
            if (hospital.GetComponent<StorageScript>().powerStored > 0)
            {
                ++poweredHospitals;
            }
        }

        return (poweredFactories > 0 && poweredHospitals > 0);

    }
}
