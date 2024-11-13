using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyObject : MonoBehaviour, IDamagalbe
{
    [SerializeField] private EnemySO data;

    private GameObject player;
    private PlayerCondition condition;

    public float health;
    public float speed;
    public float attackdamage;
    public float attackRange;
    public float attackDelay;

    private bool isAttacking = false;
    private float distance;

    private void Awake()
    {
        health = data.health;
        speed = data.speed;
        attackdamage = data.damage;
        attackRange = data.attackRange;
        attackDelay = data.attackDealy;
    }

    private void Start()
    {
        player = CharacterManager.Instance.Player.gameObject;
        condition = player.GetComponent<PlayerCondition>();
    }
    private void Update()
    {
        distance = Vector3.Distance(transform.position, player.transform.position);
        if (distance <= attackRange)
        {
            MoveToPlayer();
        }
    }

    private void MoveToPlayer()
    {
        if (distance >= 0.2f)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }
        else if (distance < 0.2f && !isAttacking)
        {
            StartCoroutine(AttackToPlayer(attackdamage));
        }
    }


    private IEnumerator AttackToPlayer(float damage)
    {
        isAttacking = true;
        Debug.Log("공격당함!");
        condition.TakePhysicalDamage(damage);
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }


    public void TakePhysicalDamage(float damage)
    {
        health -= damage;

        if (health <= 0)
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
