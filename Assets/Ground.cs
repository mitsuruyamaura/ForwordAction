using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            col.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D col) {
        if (col.gameObject.tag == "Player") {
            col.transform.SetParent(null);
        }
    }
}
