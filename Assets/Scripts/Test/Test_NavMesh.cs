using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Test_NavMesh : Test_Base
{
    NavMeshAgent agent;
    protected override void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        base.OnEnable();
        inputActions.Test.Click.performed += OnClick;
    }
    protected override void OnDisable()
    {
        inputActions.Test.Click.performed -= OnClick;
        base.OnDisable();
    }

    private void OnClick(InputAction.CallbackContext _)
    {
        Vector3 cursorPos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(cursorPos);
        Debug.Log($"CursorPos : {cursorPos}");
        if (Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            Debug.Log($"Point : {hitInfo.point}");
            agent.SetDestination(hitInfo.point); // 내가 클릭한(피킹한) 월드 좌표
        }
        
    }

}

