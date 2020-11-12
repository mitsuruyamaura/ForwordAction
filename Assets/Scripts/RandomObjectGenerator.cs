﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObjectGenerator : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objPrefab;

    [SerializeField]
    private Transform generateTran;

    [Header("生成までの待機時間")]
    public Vector2 waitTimeRange;                    // １回生成するまでの待機時間。どの位の間隔で自動生成を行うか設定

    private float waitTime;

    private float timer;                      // 待機時間の計測用


    void Start() {
        SetGenerateTime();    
    }

    /// <summary>
    /// 生成までの時間を設定
    /// </summary>
    private void SetGenerateTime() {
        // 生成までの待機時間を、最小値と最大値の間からランダムで設定
        waitTime = Random.Range(waitTimeRange.x, waitTimeRange.y);
    }

    void Update()
    {
        // 計測用タイマーを加算
        timer += Time.deltaTime;
        
        // 計測用タイマーが待機時間と同じか超えたら
        if (timer >= waitTime) {
            // タイマーをリセットして、再度計測できる状態にする
            timer = 0;

            // ランダムなオブジェクトを生成
            RandomGenerateObject();
        }
    }

    /// <summary>
    /// ランダムなオブジェクトを生成
    /// </summary>
    private void RandomGenerateObject() {

        // 生成するプレファブの番号をランダムに設定
        int randomIndex = Random.Range(0, objPrefab.Length);

        // プレファブを元にクローンのゲームオブジェクトを生成
        Instantiate(objPrefab[randomIndex], generateTran);

        // 次の生成までの時間をセットする
        SetGenerateTime();
    }
}
