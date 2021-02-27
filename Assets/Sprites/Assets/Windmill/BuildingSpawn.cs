using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingSpawn : MonoBehaviour
{
    public GameObject Building;
    public float xOffset;
    public float yOffset;

    private void Start()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        transform.localScale = new Vector3(1, 1, 1);
    }
    
    void SpawnAndDestroy()
    {
        Instantiate(Building, new Vector3(transform.position.x + xOffset, transform.position.y + yOffset, transform.position.z), Quaternion.identity);
        Destroy(gameObject);
    }

}

