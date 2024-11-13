using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyObject : MonoBehaviour
{
    public EnemySO data;


    public void TakeDamage(int damage)
    {
        data.health -= damage;

        if (data.health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");
        gameObject.SetActive(false);
    }

}
