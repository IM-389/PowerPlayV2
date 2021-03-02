using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LVPowerLine : GeneralObjectScript
{
    public void AutoAddGeneratorConnections()
    {
        Collider2D[] buildingsHit = Physics2D.OverlapCircleAll(new Vector2(transform.position.x, transform.position.y), wireLength);
        foreach(Collider2D building in buildingsHit)
        {
            if(building.transform.CompareTag("house") || building.transform.CompareTag("hospital") || building.CompareTag("factory"))
            {
                GeneralObjectScript buildingGOS = building.transform.gameObject.GetComponent<GeneralObjectScript>();
                if (consumerConnections.Count < maxLVConnections && buildingGOS.connections.Count < buildingGOS.maxLVConnections)
                {
                    AddConsumerConnection(building.gameObject);
                    buildingGOS.AddConsumerConnection(this.gameObject);
                }
            }
        }
    }
    private void Start()
    {
        AutoAddGeneratorConnections();
    }
}
