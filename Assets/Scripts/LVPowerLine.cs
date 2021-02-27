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
                AddConsumerConnection(building.gameObject);
            }
        }
    }
    private void Start()
    {
        AutoAddGeneratorConnections();
    }
}
