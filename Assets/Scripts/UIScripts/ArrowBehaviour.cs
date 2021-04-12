using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    public GameObject target;
    //public GameObject cam;
    public float speed = 1;
    float angle;
    public float destroyDis = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 targetPos = target.transform.position - cam.transform.position;
        //Vector3 dis = targetPos - gameObject.transform.position;
        Vector3 dis = target.transform.position - gameObject.transform.position;
        angle = Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, q, Time.deltaTime * speed);

        if (Vector3.Distance(gameObject.transform.position, target.transform.position) < destroyDis) //set this to whatever suits best
        {
            gameObject.SetActive(false);
        }
    }
}
