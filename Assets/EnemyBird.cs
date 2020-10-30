using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EnemyBird : MonoBehaviour
{
    public float moveTime;
    public float moveRange;


    void Start()
    {
        transform.DOMoveY(-moveRange, moveTime).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
