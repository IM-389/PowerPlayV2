using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetScale : MonoBehaviour
{
    public float xScale;
    public float yScale;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.localScale = new Vector3(xScale, yScale, 1);
    }
}
