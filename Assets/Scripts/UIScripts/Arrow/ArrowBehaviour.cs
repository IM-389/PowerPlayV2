using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBehaviour : MonoBehaviour
{
    // The target the arrow will point at if needed.
    public GameObject target;
    // The next arrow after this one.
    public GameObject next;
    //public GameObject cam;
    public float speed = 1;
    float angle;
    public float disMAX = 0.7f;
    public float disMIN = 0.3f;

    // checks if the arrow is for tracking instead of just pointing
    public bool trackingArrow;
    // checks if there is another arrow after the current one
    public bool isThereMore;
    //
    private Camera mainCam;

    private Vector3 targetPos;


    //public ArrowManager am;
    // Start is called before the first frame update
    void Start()
    {
        mainCam = Camera.main;
        targetPos = target.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //destroyDis = am.dis;
        Tracking();
    }

    void Tracking()
    {
        if (trackingArrow)
        {
            Vector3 dis = target.transform.position - gameObject.transform.position;
            angle = Mathf.Atan2(dis.y, dis.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation, q, Time.unscaledDeltaTime * speed);

            if (CheckIfFound()) //set this to whatever suits best
            {
                FinishTheJob();
            }
        }
    }

    public void FinishTheJob()
    {
        if (isThereMore && gameObject.activeSelf)
        {
            next.SetActive(true);
            isThereMore = false;
        }

        gameObject.SetActive(false);
    }

    bool CheckIfFound()
    {
        Vector2 targetScreenPos = mainCam.WorldToScreenPoint(targetPos);
        Vector2 targetViewportPos = mainCam.ScreenToViewportPoint(targetScreenPos);

        return (targetViewportPos.x <= disMAX && targetViewportPos.x >= disMIN) &&
               (targetViewportPos.y <= disMAX && targetViewportPos.y >= disMIN);
    }
}
