using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHouse : MonoBehaviour
{
    public float moveSpeed;

    private float stopPos = 6.5f;

    private GameManager gameManager;

    private bool isGoal;

    public void SetUpGoalHouse(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    void Update() {
        if (transform.position.x > stopPos) {
            transform.position += new Vector3(-moveSpeed, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) {
        if (col.gameObject.tag == "Player" && isGoal == false) {
            isGoal = true;

            // PlayerControllerの情報を取得
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();

            // GameManagerのGoalメソッドを呼び出す。引数にはPlayerControllerのcoinCountを渡す
            gameManager.Goal(playerController.coinCount);
        }
    }
}
