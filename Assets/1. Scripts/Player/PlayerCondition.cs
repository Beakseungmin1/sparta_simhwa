using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagalbe
{
    void TakePhysicalDamage(int damage);
}

public class PlayerCondition : MonoBehaviour, IDamagalbe
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition stamina { get { return uiCondition.stamina; } }

    public Condition Stamina
    {
        get { return stamina; }
    }

    public event Action onTakeDamage;

    private float lossStaminaWhileRun = 15f;

    void Update()
    {
        health.Add(health.passiveValue * Time.deltaTime);
        stamina.Add(stamina.passiveValue * Time.deltaTime);

        if (CharacterManager.Instance.Player.controller.isSprint)
        {
            stamina.Subtract(lossStaminaWhileRun * Time.deltaTime);
        }

        if (health.curValue == 0f)
        {
            Die();
        }

    }

    public void Heal(float amount)
    {
        health.Add(amount);
    }

    public void SpeedBoost(float value, float duration)
    {
        StartCoroutine(ModifyValue(value, duration));
    }

    private IEnumerator ModifyValue(float value, float duration)
    {
        CharacterManager.Instance.Player.controller.curMoveSpeed += value; // �� ����
        yield return new WaitForSeconds(duration); // ���
        CharacterManager.Instance.Player.controller.curMoveSpeed -= value; // ���� ������ ����
    }

    public void JumpBoost(float value, float duration)
    {
        StartCoroutine(ModifyValue2(value, duration));
    }

    private IEnumerator ModifyValue2(float value, float duration)
    {
        CharacterManager.Instance.Player.controller.jumpPower += value; // �� ����
        yield return new WaitForSeconds(duration); // ���
        CharacterManager.Instance.Player.controller.jumpPower -= value; // ���� ������ ����
    }

    //public void Eat(float amount)
    //{
    //    hunger.Add(amount);
    //}

    public void Die()
    {
        Debug.Log("����");
    }

    public void TakePhysicalDamage(int damage)
    {
        health.Subtract(damage);
        onTakeDamage?.Invoke();
    }
}
