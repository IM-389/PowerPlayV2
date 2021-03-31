using UnityEngine;
using Power.V2;
public class PowerManager : MonoBehaviour
{

    public enum POWER_TYPES
    {
        TYPE_COAL = 0,
        TYPE_GAS = 1,
        TYPE_SOLAR = 2,
        TYPE_WIND = 3
    }

    [Tooltip("Adjustments for coal, gas, solar, and wind respectivly")]
    public float[] powerAdjusts = {1, 1, 1, 1};
    
    /// <summary>
    /// How much power from each source is being created. Offset from 
    /// 0 is Coal
    /// 1 is Natural Gas
    /// 2 is Solar
    /// 3 is Wind
    /// </summary>
    public float[] powerAmountsGenerated = new float[4];
    // Start is called before the first frame update

    /// <summary>
    /// Get the amount of a specific type
    /// </summary>
    /// <param name="id">ID of the type to get</param>
    /// <returns>Amount of power from the given source</returns>
    public float GetAmountGenerated(POWER_TYPES type)
    {
        return powerAmountsGenerated[(int)type];
    }

    /// <summary>
    /// Change the amount of power of a given type
    /// </summary>
    /// <param name="type">Type of power to change the amount of</param>
    /// <param name="amount">The amount to change by</param>
    public void ChangeAmountGenerated(POWER_TYPES type, float amount)
    {
        powerAmountsGenerated[(int) type] += amount;
    }

    /// <summary>
    /// Calculate how much power of all types is being generated
    /// </summary>
    /// <param name="type">Type of power to calculate the amount of</param>
    public void CalculateAmountsGenerated(POWER_TYPES type)
    {
        
        // Zero the amount being recalculated
        powerAmountsGenerated[(int) type] = 0;
        
        GeneratorScript[] generators = GameObject.FindObjectsOfType<GeneratorScript>();

        foreach (var generator in generators)
        {
            if (generator.type == type)
            {
                powerAmountsGenerated[(int) generator.type] +=
                    (generator.GetComponent<PowerAmountInfo>().amountGenerated);
            }
        }
    }

}
