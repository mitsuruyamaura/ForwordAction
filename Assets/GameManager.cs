using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour
{
    public bool isGameUp;

    [SerializeField]
    private ResultPopUp resultPopUpPrefab;

    [SerializeField]
    private Transform canvasTran;


    // 未


    [SerializeField]
    private GameObject aerialFloorPrefab;

    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private GameObject coinPrefab;

    [SerializeField, HideInInspector]
    private GameObject[] generateObjs;

    [SerializeField]
    private Transform generateTran;

    [Header("生成までの待機時間")]
    public float waitTime;

    private float timer;

    public int generateCount;



    public int checkCount;

    public int wayCount;

    public int clearCount;

    public GameObject goalPrefab;

    public GameObject signPrefab;


    [SerializeField]
    private UIManager uiManager;

    [SerializeField]
    private TitleObjectController titleObjectController;

    [SerializeField]
    private StartChecker startChecker;

    private bool isTitleUp = false;

    private bool isRestart = false;


    [SerializeField]
    private AudioManager audioManager;

    IEnumerator Start() {
        isGameUp = true;
        isRestart = true;

        // タイトル曲再生
        StartCoroutine(audioManager.PlayBGM(0));

        // タイトル表示
        uiManager.SwitchDisplayTitle(true, 1.0f);

        isTitleUp = true;

        // タップを待つ
        yield return new WaitUntil(() => isTitleUp == false);

        // タイトル曲を終了し、ゲーム内曲を再生
        StartCoroutine(audioManager.PlayBGM(1));

        // タイトルを非表示
        uiManager.SwitchDisplayTitle(false, 0);

        // ゴール地点を画面外に移動させる
        titleObjectController.MoveTitleObject();

        // スタート地点を少しずつ画面外に移動させる
        //startChecker.SetInitialSpeed();

        // ゲームスタート表示
        yield return StartCoroutine(uiManager.DisplayGameStartInfo());

        isGameUp = false;
        isRestart = false;
    }



    void Update()
    {
        // タイトル表示中
        if (isTitleUp == true) {

            // タップするか、スペースを押すと
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {
                // タイトル表示を終了させる制御にうつる
                isTitleUp = false;
            }
        }

        if (isGameUp == true && isTitleUp == false) {

            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)) {

                if (isRestart == false) {
                    isRestart = true;
                    uiManager.RestartGame();
                }          
            }
        }

        if (isGameUp == true) {
            return;
        }

        if (wayCount < clearCount) {
            timer += Time.deltaTime;

            if (timer >= waitTime) {
                timer = 0;
                Generate();
            }
        }
    }

    /// <summary>
    /// プレファブを元にクローンのゲームオブジェクトを生成
    /// </summary>
    private void Generate() {
        generateCount++;

        // 種類
        //int randomVelue = Random.Range(0, 100);

        //GameObject randomObj = null;
        //if (randomVelue < 30) {
        //    randomObj = generateObjs[0];
        //} else if (randomVelue >= 30 && randomVelue < 60) {
        //    randomObj = generateObjs[1];
        //} else {
        //    randomObj = generateObjs[2];
        //}
        //GameObject obj = Instantiate(randomObj, generateTran);

        GameObject obj = Instantiate(aerialFloorPrefab, generateTran);    // generateObjs[0]

        // 位置
        float randomPosY = Random.Range(-3.8f, 3.8f);

        obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + randomPosY);

        // 生成回数が指定数を超えたら
        //if(generateCount > checkCount) {
        //    // 0に戻す
        //    generateCount = 0;

        //    // チェックポイントかゴールを生成
        //    CheckPoint();
        //}
    }

    /// <summary>
    /// チェックポイントかゴールを生成の判定
    /// </summary>
    private void CheckPoint() {
        wayCount++;

        if (wayCount >= clearCount) {
            GenerateGoal();
        } else {
            GenerateSign();
        }
    }

    /// <summary>
    /// ゴール生成
    /// </summary>
    private void GenerateGoal() {
        GameObject goal = Instantiate(goalPrefab);
        GoalChecker goalHouse = goal.GetComponent<GoalChecker>();
        goalHouse.SetUpGoalHouse(this);
    }

    /// <summary>
    /// 中間地点の看板生成
    /// </summary>
    private void GenerateSign() {
        Instantiate(signPrefab);
    }

    /// <summary>
    /// ゴール到着
    /// </summary>
    public void Goal(int score) {
        isGameUp = true;

        // クリアの曲再生
        StartCoroutine(audioManager.PlayBGM(2));

        //GameObject resultPopUp = Instantiate(ResultPopUpPrefab, canvasTran, false);
        //resultPopUp.GetComponent<ResultPopUp>().SetUpResultPopUp(score);

        ResultPopUp resultPopUp = Instantiate(resultPopUpPrefab, canvasTran, false);
        resultPopUp.SetUpResultPopUp(score);
    }

    /// <summary>
    /// ゲームオーバー処理
    /// </summary>
    public void GameOver() {
        Debug.Log("Game Over");

        isGameUp = true;

        // ゲームオーバー表示
        uiManager.DisplayGameOverInfo();

        StartCoroutine(audioManager.PlayBGM(3));
    }
}
