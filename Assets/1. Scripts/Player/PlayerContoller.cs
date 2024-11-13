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
    private float defaultspeed;
    public float RunnincreaseSpeed;
    public float jumpPower;
    private float curJumpCount;
    private float JumpCountLimit;
    private Vector2 curMovementInput;
    public LayerMask groundLayerMask;

    [Header("Look")]
    public Transform CameraContainer;
    public GameObject FirstCamera;
    public GameObject ThirdCamera;
    public float minXLook;
    public float maxXLook;
    private float camCurXRot;
    public float lookSensitivity;
    private Vector2 mouseDelta;
    public bool canLook = true;
    public bool isSprint = false;

    public Action inventory;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void LateUpdate()
    {
        if (canLook)
        CameraLook();
    }

    private void Move()
    {
        Vector3 dir = transform.forward * curMovementInput.y + transform.right * curMovementInput.x;
        dir *= curMoveSpeed;
        dir.y = _rigidbody.velocity.y;

        _rigidbody.velocity = dir;
    }

    void CameraLook()
    {
        camCurXRot += mouseDelta.y * lookSensitivity;
        camCurXRot = Mathf.Clamp(camCurXRot, minXLook, maxXLook);
        CameraContainer.localEulerAngles = new Vector3(-camCurXRot, 0, 0);

        transform.eulerAngles += new Vector3(0, mouseDelta.x * lookSensitivity, 0);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            curMovementInput = context.ReadValue<Vector2>();
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMovementInput = Vector2.zero;
        }
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && IsGrounded() || context.phase == InputActionPhase.Started && curJumpCount < JumpCountLimit)
        {
            _rigidbody.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            curJumpCount++;
        }
        if (IsGrounded())
        {
            curJumpCount = 0;
        }
    }

    public void AddJump(float value)
    {
        JumpCountLimit += value;
    }

    public void DisCountJump(float value)
    {
        JumpCountLimit -= value;
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && CharacterManager.Instance.Player.condition.Stamina.curValue > 0f)
        {
            defaultspeed = curMoveSpeed;
            curMoveSpeed += RunnincreaseSpeed;
            isSprint = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            curMoveSpeed = defaultspeed;
            isSprint = false;
        }
    }

    public void OnChangeThirdView(InputAction.CallbackContext context)
    {
        if(isThird())
        {
            ThirdCamera.SetActive(false);
            FirstCamera.SetActive(true);
        }
        else
        {
            ThirdCamera.SetActive(true);
            FirstCamera.SetActive(false);
        }
    }

    public bool isThird()
    {
        return ThirdCamera.activeInHierarchy;
    }

    bool IsGrounded()
    {
        Ray[] rays = new Ray[4]
        {
            new Ray(transform.position + (transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.forward * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down),
            new Ray(transform.position + (-transform.right * 0.2f) + (transform.up * 0.01f), Vector3.down)
        };

        for (int i = 0; i < rays.Length; i++)
        {
            if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
            {
                return true;
            }
        }

        return false;
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Started)
        {
            inventory?.Invoke();
            ToggleCursor();
        }
    }

    void ToggleCursor()
    {
        bool toggle = Cursor.lockState == CursorLockMode.Locked;
        Cursor.lockState = toggle ? CursorLockMode.None : CursorLockMode.Locked;
        canLook = !toggle;
    }

}
