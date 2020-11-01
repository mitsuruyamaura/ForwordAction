using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleObjectController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleObj;

    public void MoveObject() {
        // ゲームスタートに合わせてゴールを画面の右端側に移動させて、画面から見えなくなってから非表示にする
        titleObj.transform.DOMoveX(15, 2.0f).SetEase(Ease.Linear).OnComplete(() => { titleObj.SetActive(false); });
    }
}
