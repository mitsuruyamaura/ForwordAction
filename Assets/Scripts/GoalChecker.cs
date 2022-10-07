using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoalChecker : MonoBehaviour
{
    public float moveSpeed;

    private float stopPos = 6.5f;

    private bool isGoal;

    [SerializeField]
    private GameManager gameManager;

    private GameDirector gameDirector;

    //　未

    [SerializeField]
    private GameObject secretfloorObj;


    /// <summary>
    /// ゴール地点の設定1 
    /// </summary>
    /// <param name="gameManager"></param>
    public void SetUpGoalHouse(GameManager gameManager) {
        this.gameManager = gameManager;

        // 落下防止の床を非表示
        secretfloorObj.SetActive(false);
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

            Debug.Log("ゲームクリア");
            // PlayerControllerの情報を取得
            PlayerController playerController = col.gameObject.GetComponent<PlayerController>();

            playerController.uiManager.GenerateRusultPopUp(playerController.coinPoint);

            // ゴール到着
            gameDirector.GoalClear();

            // GameManagerのGoalメソッドを呼び出す。引数にはPlayerControllerのcoinCountを渡す
            //gameManager.Goal(playerController.coinPoint);

            // 落下防止の床を表示
            secretfloorObj.SetActive(true);

            // 落下防止の床を画面下からアニメさせて表示
            secretfloorObj.transform.DOLocalMoveY(0.45f, 2.5f).SetEase(Ease.Linear).SetRelative().SetLink(gameObject);
        }
    }

    /// <summary>
    /// ゴール地点の設定(GameDirector用)
    /// </summary>
    public void SetUpGoalHouse(GameDirector gameDirector) {

        this.gameDirector = gameDirector;

        // 落下防止の床を非表示
        secretfloorObj.SetActive(false);
    }
}
