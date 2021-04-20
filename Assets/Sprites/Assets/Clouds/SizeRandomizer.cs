using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizeRandomizer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = gameObject.transform.localScale * Random.Range(1f, 1.2f);
    }

}
