using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    PlayerInputActions inputActions;
    Rigidbody2D rigid;
    Animator anim;

    Vector2 inputDir;
    float moveSpeed= 3.0f;
    private void Awake()
    {
        inputActions = new PlayerInputActions();
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnStop;
        inputActions.Player.Attack.performed += OnAttack;
       
    }


    private void OnDisable()
    {
        inputActions.Player.Attack.performed -= OnAttack;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Move.performed -= OnStop;
        inputActions.Player.Disable();
    }

    private void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + Time.fixedDeltaTime * moveSpeed * inputDir);
    }
    private void OnMove(InputAction.CallbackContext context)
    {
        inputDir = context.ReadValue<Vector2>();
        anim.SetFloat("InputX", inputDir.x);
        anim.SetFloat("InputY", inputDir.y);
        anim.SetBool("Move", true);
    }
    private void OnStop(InputAction.CallbackContext obj)
    {
        inputDir = Vector2.zero;
        anim.SetBool("Move", false);
    }
    private void OnAttack(InputAction.CallbackContext _)
    {

    }

}
