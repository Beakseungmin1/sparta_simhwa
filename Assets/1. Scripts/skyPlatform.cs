using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class skyPlatform : MonoBehaviour
{
    public Transform targetPosition;
    public Transform startPosition;
    public float speed;
    private bool isArrive = false;


    void Update()
    {
        if (!isArrive)
        {
            MoveToTarget();
            if (Vector3.Distance(transform.position, targetPosition.position) < 0.2f)
            {
                isArrive = true;
            }
        }
        else
        {
            MoveToStart();
            if (Vector3.Distance(transform.position, startPosition.position) < 0.2f)
            {
                isArrive = false;
            }
        }
    }

    public void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPosition.position, speed * Time.deltaTime);
    }

    public void MoveToStart()
    {
        transform.position = Vector3.MoveTowards(transform.position, startPosition.position, speed * Time.deltaTime);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.position = Vector3.MoveTowards(collision.transform.position, transform.position, speed * Time.deltaTime);
        }
    }

}
