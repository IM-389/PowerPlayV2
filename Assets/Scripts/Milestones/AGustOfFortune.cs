using System.Collections;
using UnityEngine;
using Power.V2;

namespace Milestones
{
    public class AGustOfFortune : MilestoneBase
    {
        [Tooltip("How many solars need to be upgraded to complete the milestone")]
        public int numToComplete = 5;
        
        public override bool CheckCompleteMilestone()
        {
            GeneratorScript[] allGenerators = GameObject.FindObjectsOfType<GeneratorScript>();

            int upgradedWind = 0;

            foreach (var generator in allGenerators)
            {
                if (generator.type == PowerManager.POWER_TYPES.TYPE_WIND && generator.GetComponent<GeneralObjectScript>().isSmart)
                {
                    ++upgradedWind;
                }
            }

            return upgradedWind >= numToComplete;

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