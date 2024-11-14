using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagalbe
{
    void TakePhysicalDamage(float damage);
}

public class PlayerCondition : MonoBehaviour, IDamagalbe
{
    public UICondition uiCondition;

    Condition health { get { return uiCondition.health; } }
    Condition Mana { get { return uiCondition.stamina; } }
    Condition Exp {  get { return uiCondition.exp; } }

    void Update()
    {

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
        CharacterManager.Instance.Player.controller.curMoveSpeed += value; // 값 증가
        yield return new WaitForSeconds(duration); // 대기
        CharacterManager.Instance.Player.controller.curMoveSpeed -= value; // 원래 값으로 복구
    }

    public void AddExp(float amount)
    {
        Exp.Add(amount);
    }



    //public void Eat(float amount)
    //{
    //    hunger.Add(amount);
    //}

    public void Die()
    {
        Debug.Log("죽음");
    }

    public void TakePhysicalDamage(float damage)
    {
        health.Subtract(damage);
    }
}
