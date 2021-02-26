using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedRemoval : MonoBehaviour
{
    [Tooltip("How long to let the object live")]
    public float lifetime;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}