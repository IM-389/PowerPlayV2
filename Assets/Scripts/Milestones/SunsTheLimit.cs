using System.Collections;
using Power.V2;
using UnityEngine;

namespace Milestones
{
    public class SunsTheLimit : MilestoneBase
    {
        [Tooltip("How many solars need to be upgraded to complete the milestone")]
        public int numToComplete = 5;
        
        public override bool CheckCompleteMilestone()
        {
            GeneratorScript[] allGenerators = GameObject.FindObjectsOfType<GeneratorScript>();

            int upgradedSolar = 0;

            foreach (var generator in allGenerators)
            {
                if (generator.type == PowerManager.POWER_TYPES.TYPE_SOLAR && generator.GetComponent<GeneralObjectScript>().isSmart)
                {
                    ++upgradedSolar;
                }
            }

            return upgradedSolar >= numToComplete;

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
                    if (generator.type == PowerManager.POWER_TYPES.TYPE_SOLAR && !generator.GetComponent<GeneralObjectScript>().isSmart)
                    {
                        generator.DoUpgrade();
                    }
                }
                yield return wait;
            }
        }
    }
}