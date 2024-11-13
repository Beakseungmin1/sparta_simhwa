using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Enemy", menuName = "New Enemy")]

public class EnemySO : ScriptableObject
{

    [Header("Info")]

    public float health;
    public float speed;
    public float damage;
}
