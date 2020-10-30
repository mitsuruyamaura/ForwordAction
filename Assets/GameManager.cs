using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject[] generateObjs;

    public Transform generateTran;

    public float waitTime;

    private float timer;

    public int generateCount;

    public int checkCount;

    public int wayCount;

    public int clearCount;

    public GameObject goalPrefab;

    public GameObject signPrefab;

    public bool isGameUp;

    public GameObject ResultPopUpPrefab;

    public Transform canvasTran;


    void Update()
    {
        if (isGameUp == true) {
            return;
        }

        timer += Time.deltaTime;

        if (timer >= waitTime) {
            timer = 0;
            Generate();
        }   
    }

    private void Generate() {
        generateCount++;

        // 種類
        int randomVelue = Random.Range(0, 100);

        GameObject randomObj = null;
        if (randomVelue < 30) {
            randomObj = generateObjs[0];
        } else if (randomVelue >= 30 && randomVelue < 60) {
            randomObj = generateObjs[1];
        } else {
            randomObj = generateObjs[2];
        }
        GameObject obj = Instantiate(randomObj, generateTran);

        // 位置
        float randomPosY = Random.Range(-4.0f, 4.0f);

        obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + randomPosY);

        // 生成回数が指定数を超えたら
        if(generateCount > checkCount) {
            // 0に戻す
            generateCount = 0;

            // チェックポイントかゴールを生成
            CheckPoint();
        }
    }

    /// <summary>
    /// チェックポイントかゴールを生成の判定
    /// </summary>
    private void CheckPoint() {
        wayCount++;

        if (wayCount >= clearCount) {
            isGameUp = true;
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
        GoalHouse goalHouse = goal.GetComponent<GoalHouse>();
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
        GameObject resultPopUp = Instantiate(ResultPopUpPrefab, canvasTran, false);
        resultPopUp.GetComponent<ResultPopUp>().SetUpResultPopUp(score);
    }
}
