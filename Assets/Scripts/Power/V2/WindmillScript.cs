using System.Collections;
using UnityEngine;

namespace Power.V2
{
    public class WindmillScript : GeneratorScript
    {
        [Tooltip("How large an area upgraded windmills affect")]
        public Vector2 farmSize = new Vector2(3, 3);

        [Tooltip("Amount each windmill in the area boosts by")]
        public float windmillMultiplier = 2;

        private float originalPower;

        private bool isUpgraded;
        public override void DoUpgrade()
        {
            if (isUpgraded)
                return;

            isUpgraded = true;
            Debug.Log("Upgrading Windmill!");
            originalPower = basePower;
            StartCoroutine(WindmillCheck());
        }

        private IEnumerator WindmillCheck()
        {
            WaitForSeconds wait = new WaitForSeconds(0.5f);
            while (true)
            {
                int numWindmills = 0;
                Collider2D[] overlapped = Physics2D.OverlapBoxAll(transform.position, farmSize, 0);
                foreach (var item in overlapped)
                {
                    if (item.CompareTag("Generator") && item.GetComponent<GeneratorScript>().type == PowerManager.POWER_TYPES.TYPE_WIND)
                    {
                        ++numWindmills;
                    }
                }

                basePower = originalPower + (windmillMultiplier * numWindmills);
                yield return wait;
            }
        }
    }
}