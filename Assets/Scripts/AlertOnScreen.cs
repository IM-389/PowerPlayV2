using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlertOnScreen : MonoBehaviour
{
    Vector3 startPos;
    public float minStayOnX = 0.1f;
    public float maxStayOnX = 0.9f;
    public float minStayOnY = 0.1f;
    public float maxStayOnY = 0.9f;
    private void Start()
    {
        startPos = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = startPos;
        Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
        pos.x = Mathf.Clamp(pos.x , minStayOnX, maxStayOnX);
        pos.y = Mathf.Clamp(pos.y , minStayOnY, maxStayOnY);
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}
