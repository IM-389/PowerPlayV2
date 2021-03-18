using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertOnScreen : MonoBehaviour
{
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = startPos;
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x , 0.1f, 0.9f);
        pos.y = Mathf.Clamp(pos.y , 0.1f, 0.9f);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
