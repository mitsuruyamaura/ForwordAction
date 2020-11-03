using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoalHouse : MonoBehaviour
{
    public float moveSpeed;

    [SerializeField]
    private GameObject secretfloor;

    private float stopPos = 6.5f;

    private GameManager gameManager;

    private bool isGoal;

    public void SetUpGoalHouse(GameManager gameManager) {
        this.gameManager = gameManager;

        // 落下防止の床を非表示
        secretfloor.SetActive(false);
    }

    void Update() {
        if (transform.position.x > stopPos) {
            transform.position += new Vector3(-moveSpeed, 0, 0);
        } 
    }

    private void OnTriggerEnter2D(Collider2D col) {
        // ゴールした際に１回だけ判定する
        if (col.gameObject.tag == "Player" && isGoal == false) {
            isGoal = true;

            // PlayerControllerの情報を取得
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();

            // GameManagerのGoalメソッドを呼び出す。引数にはPlayerControllerのcoinCountを渡す
            gameManager.Goal(playerController.coinCount);

            // 落下防止の床を表示
            secretfloor.SetActive(true);

            // 落下防止の床を画面下からアニメさせて表示
            secretfloor.transform.DOLocalMoveY(0.45f, 2.5f).SetEase(Ease.Linear).SetRelative();
        }
    }
}
