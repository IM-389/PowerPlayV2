using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WireBreakEvent : EventBase
{
    // Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    public BuildScript wireBreaker;
    private MoneyManager moneyManager;
    public override void DoEvent()
    {
        Debug.Log("Wire Break Selected!");
        //Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        int chance = Random.Range(0, 100);
        //pick a random item, pick a random connection, remove
        if (chance < eventChance)
        {
            Debug.Log("Wire Break is running");
            GeneralObjectScript[] breakConnection = GameObject.FindObjectsOfType<GeneralObjectScript>();//find types-in this case GOS, find all enabled GOS in scene
            int rand = Random.Range(0, breakConnection.Length);//object A
            List<GameObject> storeBreak = breakConnection[rand].GetAllConnections();
            //now have list of all connected objects-pick one at random
            int randTwo = Random.Range(0, storeBreak.Count);//objectB
            //cache
            GameObject storeCache = storeBreak[randTwo];
            //Remove A from B's connection list and vice versa
            storeCache.GetComponent<GeneralObjectScript>().RemoveConnection(breakConnection[rand].gameObject);//removes A from B
            breakConnection[rand].RemoveConnection(storeCache);
            
        }
    }
}
