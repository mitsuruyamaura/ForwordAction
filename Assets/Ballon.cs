using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ballon : MonoBehaviour
{
    private PlayerController playerController;

    public void SetUpBallon(PlayerController playerController) {
        this.playerController = playerController;
    }

    private void OnCollisionEnter2D(Collision2D col) {
        if (col.gameObject.tag == "Enemy") {
            playerController.DestroyBallon(this);
        }
    }
}
