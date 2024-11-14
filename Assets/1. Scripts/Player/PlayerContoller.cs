using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class PlayerContoller : MonoBehaviour
{
    [Header("Moverment")]
    public float curMoveSpeed;
    public bool isFind = false;

    public float Damage;
    public float detectionRange;
    public float attackDealy;

    private bool isAttacking = false;
    private bool isFindingEnemy = false;

    private Transform startPosition;
    public Transform targetPosition;
    public Action inventory;
    public Action Shop;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startPosition = transform;
        StartCoroutine(DetectEnemysCoroutine());
    }

    private void Update()
    {
        if (!isFindingEnemy)
        {
            MoveToPoint();
        }
    }

    private IEnumerator DetectEnemysCoroutine()
    {
        while (true)
        {
            DetectEnemy();
            yield return new WaitForSeconds(1f);
        }
    }

    void DetectEnemy()
    {
        GameObject[] Enemys = GameObject.FindGameObjectsWithTag("Enemy");
        bool enemyDetected = false;

        foreach (GameObject Enemy in Enemys)
        {
            float distance = Vector3.Distance(transform.position, Enemy.transform.position);
            if (distance <= detectionRange)
            {
                EnemyObject enemy = Enemy.GetComponent<EnemyObject>();
                if (Enemy.activeInHierarchy && !isAttacking)
                {
                    StartCoroutine(AttackEnemy(enemy));
                    enemyDetected = true;
                    break;
                }
            }
        }
        isFindingEnemy = enemyDetected;
    }

    private IEnumerator AttackEnemy(EnemyObject enemy)
    {
        isAttacking = true;
        enemy.TakePhysicalDamage(Damage);
        Debug.Log(Damage + "의 데미지로 공격 " + enemy.name);
        yield return new WaitForSeconds(attackDealy);
        isAttacking = false;

        if (!enemy.gameObject.activeInHierarchy)
        {
            isFindingEnemy = false;
        }
    }

    public void MoveToPoint()
    {
        transform.position = Vector3.MoveTowards(startPosition.position, targetPosition.position, curMoveSpeed * Time.deltaTime);
    }



    public void OnInventory()
    {
        inventory?.Invoke();
    }

    public void OnShop()
    {
        Shop?.Invoke();
    }

}
