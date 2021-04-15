using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSpawner : MonoBehaviour
{
    public GameObject TalkParticles;
    public GameObject GriddieHead;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void spawnParticles()
    {
        Instantiate(TalkParticles, GriddieHead.transform.position + new Vector3(1.4f,0,0), Quaternion.identity);
    }
}
