using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumping_plat : MonoBehaviour
{
    Rigidbody Rigidbody;
    private float jumpingPower = 600;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rigidbody = collision.gameObject.GetComponent<Rigidbody>();

            Rigidbody.AddForce(Vector3.up * jumpingPower, ForceMode.Impulse);
        }
    }
}
