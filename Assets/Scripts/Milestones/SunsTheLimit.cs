using System.Collections;
using Power.V2;
using UnityEngine;

namespace Milestones
{
    public class SunsTheLimit : MilestoneBase
    {
        [Tooltip("How many solars need to be upgraded to complete the milestone")]
        public int numToComplete = 5;

        private PowerManager powerManager;

        private void Start()
        {
            powerManager = GameObject.FindWithTag("GameController").GetComponent<PowerManager>();
        }
        
        
        public override bool CheckCompleteMilestone()
        {
            return powerManager.powerAmountsGenerated[2] >= 150;
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