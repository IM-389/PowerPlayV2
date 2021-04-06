using UnityEngine;

namespace Power.V2
{
    public class SolarScript : GeneratorScript
    {
        [Tooltip("How much power is generated when upgraded")]
        public float upgradePowerAmount;
        public override void DoUpgrade()
        {
            Debug.Log("Upgrading solar panel!");
            basePower = upgradePowerAmount;
            powerAdjustImmune = true;
        }
    }
}