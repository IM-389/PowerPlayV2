using UnityEngine;

namespace Power.V1
{
    public class SubstationScript : MonoBehaviour
    {

        private GeneralObjectScript gos;

        public bool voltageSet;

        [Tooltip("How many connections the substation can have")]
        public int maxConnections;

        // Start is called before the first frame update
        void Start()
        {
            gos = gameObject.GetComponent<GeneralObjectScript>();
        }

        private void Update()
        {
            if (gos.nonConsumerConnections.Count <= 0 && gos.nonConsumerConnections.Count <= 0)
            {
                voltageSet = false;
                //gos.maxLVConnections = 1;
                gos.maxConnections = 1;
            }

            if (!voltageSet)
            {
                if (gos.nonConsumerConnections.Count > 0)
                {
                    gos.volts = GeneralObjectScript.Voltage.HIGH;
                    gos.maxConnections = maxConnections;
                    //gos.maxLVConnections = 0;
                    voltageSet = true;
                }
                /*
                else if (gos.lvConnections.Count > 0)
                {
                    gos.volts = GeneralObjectScript.Voltage.LOW;
                    //gos.maxLVConnections = maxConnections;
                    gos.maxConnections = 0;
                    voltageSet = true;
                }
                */

            }

        }
    }
}
