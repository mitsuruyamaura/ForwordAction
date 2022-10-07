using System.Collections;
using UnityEngine;
using DG.Tweening;

public class TitleObjectController : MonoBehaviour
{
    [SerializeField]
    private GameObject titleObj;

    [SerializeField, HideInInspector]
    private GameDirector gameDirector;

    [SerializeField]
    private UIManager uiManager;

    IEnumerator Start() {
        //yield return new WaitUntil(() => gameDirector.isSetUp);

        yield return new WaitUntil(() => uiManager.isTitleClicked);
        
        MoveTitleObject();
    }

    /// <summary>
    /// TitleObject を移動
    /// </summary>
    public void MoveTitleObject() {
        // ゲームスタートに合わせてゴールを画面の右端側に移動させて、画面から見えなくなってから非表示にする
        titleObj.transform.DOMoveX(15, 2.0f).SetEase(Ease.Linear).SetLink(gameObject).OnComplete(() => { titleObj.SetActive(false); });
    }
}
