using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.InputSystem;

public class PlayerContoller : MonoBehaviour
{
    [Header("Moverment")]
    public float curMoveSpeed;
    public bool isFind = false;


    private Transform startPosition;
    public Transform targetPosition;
    public Action inventory;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        startPosition = transform;
    }

    private void Update()
    {
        if (!isFind)
        {
            MoveToPoint();
        }
    }

    
    public void Findzombie()
    {

    }

    public void MoveToPoint()
    {
        transform.position = Vector3.MoveTowards(startPosition.position, targetPosition.position, curMoveSpeed * Time.deltaTime);
    }

    

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
        }
    }

}
