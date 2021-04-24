using System.Collections;
using UnityEngine;
using Power.V2;

namespace Milestones
{
    public class AGustOfFortune : MilestoneBase
    {
        [Tooltip("How many solars need to be upgraded to complete the milestone")]
        public int numToComplete = 5;

        private int turbineStart;

        private void Start()
        {
            GeneratorScript[] allGenerators = GameObject.FindObjectsOfType<GeneratorScript>();

            foreach (var generator in allGenerators)
            {
                if (generator.type == PowerManager.POWER_TYPES.TYPE_WIND)
                {
                    ++turbineStart;
                }
            }
        }
        
        
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

            GeneratorScript[] allGenerators = GameObject.FindObjectsOfType<GeneratorScript>();

            int turbines = 0;

            foreach (var generator in allGenerators)
            {
                if (generator.type == PowerManager.POWER_TYPES.TYPE_WIND)
                {
                    ++turbines;
                }
            }

            return (poweredFactories > 6 && poweredHospitals > 2) && (turbines >= (turbineStart + numToComplete));
        }
        
        public override void SetCompleteMilestone()
        {
            base.SetCompleteMilestone();
            StartCoroutine(AutoUpgrade());

        }
        
        private IEnumerator AutoUpgrade()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            while (true)
            {
                GeneratorScript[] allGenerators = GameObject.FindObjectsOfType<GeneratorScript>();
                

                foreach (var generator in allGenerators)
                {
                    if (generator.type == PowerManager.POWER_TYPES.TYPE_WIND && !generator.GetComponent<GeneralObjectScript>().isSmart)
                    {
                        generator.DoUpgrade();
                    }
                }
                yield return wait;
            }
        }
    }
}