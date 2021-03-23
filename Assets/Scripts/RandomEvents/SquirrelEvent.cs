using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquirrelEvent : EventBase
{
    // Start is called before the first frame update
    // void Start()
    //{
    //find all gameobjects with tag Power
    //determine if consumer connections has items-if so, valid target
    //for each through the consumer connections, then do the same A/B removal i did in wirebreak
    //except ofc this is for each object in the powerpole consumer connection list(called consumerConnections)
    //variables are camelcased, functions pascal cased
    // }
    //public GameObject[] findPower;
    private BuildScript wireObject1;
    public override bool DoEvent()
    {
        Debug.Log("Squirrel time selected!");
        int chance = Random.Range(0, 100);
        if(chance < eventChance)
        {
            Debug.Log("Squirrel time baby!");
           
            GameObject[] findPower = GameObject.FindGameObjectsWithTag("Power");//finds all gameobjects with the tag "power"
            int rand = Random.Range(0, findPower.Length);
            GeneralObjectScript safeFound = findPower[rand].GetComponent<GeneralObjectScript>();
            if (safeFound.consumerConnections.Count > 0)
            {
                for(int i = 0; i < safeFound.consumerConnections.Count;)
                {
                    safeFound.consumerConnections[i].GetComponent<GeneralObjectScript>().RemoveConnection(safeFound.gameObject);
                    safeFound.RemoveConnection(safeFound.consumerConnections[i]);
                    //Destroy(wireObject1);
                    //if(wireObject1.wireObject1.GetComponentInChildren<SpriteRenderer>().color == Color.blue)
                    //{
                    //    Destroy(wireObject1.wireObject1);
                   // }
                }
            }

            return true;

        }

        return false;
    }
    
    

}
