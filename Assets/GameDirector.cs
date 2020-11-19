using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDirector : MonoBehaviour
{
    // ゴール
    [SerializeField]
    private GoalChecker goalHousePrefab;

    // Player
    [SerializeField]
    private PlayerController playerController;

    [SerializeField]
    private FloorGenerator[] floorGenerators;

    // ジェネレータ関連
    [SerializeField]
    private RandomObjectGenerator[] randomObjectGenerators;

    [SerializeField]
    private AudioManager audioManager;

    // ゲームスタートフラグ
    private bool isSetUp;

    // ゲーム終了フラグ
    private bool isGameUp;

    // 生成回数
    private int generateCount;

    // クリア回数
    public int clearCount;

    // プロパティ
    public int GenerateCount
    {
        set {
            generateCount = value;

            Debug.Log("生成数 / クリア目標数 : " + generateCount + " / " + clearCount);

            if (generateCount >= clearCount) {
                // ゴール地点を生成
                GenerateGoal();

                // ゲーム終了
                GameUp();
            }
        }
        get {
            return generateCount;
        }
    }

    void Start()  {

        // タイトル曲再生
        StartCoroutine(audioManager.PlayBGM(0));

        // ゲーム開始状態にセット
        isGameUp = false;
        isSetUp = false;

        // FloorGeneratorの準備
        SetUpFloorGenerators();

        // 各ジェネレータを停止
        StopGenerators();
    }

    /// <summary>
    /// FloorGeneratorの準備
    /// </summary>
    private void SetUpFloorGenerators() {
        for (int i = 0; i < floorGenerators.Length; i++) {
            floorGenerators[i].SetUpGenerator(this);
        }
    }

    /// <summary>
    /// 各ジェネレータを停止する(１つにまとめる課題を作る)
    /// </summary>
    private void StopGenerators() {
        for (int i = 0; i < randomObjectGenerators.Length; i++) {
            randomObjectGenerators[i].SwitchActivation(false);
        }

        for (int i = 0; i < floorGenerators.Length; i++) {
            floorGenerators[i].SwitchActivation(false);
        }
    }

    void Update() {
        // プレイヤーがはじめてバルーンを生成したら
        if (playerController.isFirstGenerateBallon && isSetUp == false) {

            // 準備完了
            isSetUp = true;

            // 各ジェネレータを動かし始める
            ActivateGenerators();

            // タイトル曲を終了し、ゲーム内曲を再生
            StartCoroutine(audioManager.PlayBGM(1));
        }
    }

    /// <summary>
    /// 各ジェネレータを動かし始める
    /// </summary>
    private void ActivateGenerators() {
        for (int i = 0; i < randomObjectGenerators.Length;i++) {
            randomObjectGenerators[i].SwitchActivation(true);
        }

        for (int i = 0; i < floorGenerators.Length;i++) {
            floorGenerators[i].SwitchActivation(true);
        }
    }

    /// <summary>
    /// ゴール地点の生成
    /// </summary>
    private void GenerateGoal() {
        Debug.Log("ゴール地点 生成");

        // ゴール地点を生成
        GoalChecker goalHouse = Instantiate(goalHousePrefab);

        // ゴール地点の設定
        goalHouse.SetUpGoalHouse(this);
    }

    /// <summary>
    /// ゲーム終了
    /// </summary>
    public void GameUp() {

        // ゲーム終了
        isGameUp = true;

        // 各ジェネレータを停止
        StopGenerators();
    }

    /// <summary>
    /// ゴール到着
    /// </summary>
    public void GoalClear() {
        // クリアの曲再生
        StartCoroutine(audioManager.PlayBGM(2));
    }
}
