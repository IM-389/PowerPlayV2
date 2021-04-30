using System.Collections;
using Power.V2;
using UnityEngine;

namespace Milestones
{
    public class SunsTheLimit : MilestoneBase
    {
        [Tooltip("How many solars need to be placed to complete the milestone")]
        public int numToComplete = 5;

        private int currentSolars;

        private void Start()
        {
            GeneratorScript[] generators = GameObject.FindObjectsOfType<GeneratorScript>();

            foreach (var generator in generators)
            {
                if (generator.type == PowerManager.POWER_TYPES.TYPE_SOLAR)
                {
                    ++currentSolars;
                }
                
            }
        }


        public override bool CheckCompleteMilestone()
        {
            GeneratorScript[] generators = GameObject.FindObjectsOfType<GeneratorScript>();

            int solars = 0;
            
            foreach (var generator in generators)
            {
                if (generator.type == PowerManager.POWER_TYPES.TYPE_SOLAR)
                {
                    ++solars;
                }
            }
            
            GameObject[] allHouses = GameObject.FindGameObjectsWithTag("house");

            int poweredHouses = 0;
            
            foreach (var house in allHouses)
            {
                // If the house is powered
                    if (house.GetComponent<GeneralObjectScript>().isMilestoneCounted &&
                        house.GetComponent<ConsumerScript>().GetManager().hasEnoughPower)
                    {
                        ++poweredHouses;
                    }
            }

            Debug.Log($"[SunsTheLimit] Current Solars: {solars}");
            Debug.Log($"[SunsTheLimit] Powered Houses: {poweredHouses}");
            return solars >= (currentSolars + numToComplete) && poweredHouses >= 45;
        }

        public override void SetCompleteMilestone()
        {
            base.SetCompleteMilestone();
            
            GeneratorScript[] generators = GameObject.FindObjectsOfType<GeneratorScript>();

            foreach (var generator in generators)
            {
                if (generator.type == PowerManager.POWER_TYPES.TYPE_WIND)
                {
                    generator.DoUpgrade();
                }
                
            }
            
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