using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartObject : MonoBehaviour
{
    [SerializeField]
    private MoveObject moveObject;

    public void StartGame() {
        moveObject.moveSpeed = 0.005f;
    }
}
