﻿using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class StartChecker : MonoBehaviour
{
    //[SerializeField]
    private MoveObject moveObject;

    [SerializeField]
    private PlayerController playerController;

    void Start() {
        moveObject = GetComponent<MoveObject>();    
    }

    /// <summary>
    /// 空中床に移動速度を与える
    /// </summary>
    public void SetInitialSpeed() {
        moveObject.moveSpeed = 0.005f;

        playerController.GameStart();
    }
}