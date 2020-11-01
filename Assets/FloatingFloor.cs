using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FloatingFloor : MonoBehaviour
{
    private Rigidbody2D rb;

    private Vector2 pos;

    private float randomValue;

    void Start()
    {
        pos = transform.position;
        //rb = GetComponent<Rigidbody2D>();
        randomValue = Random.Range(0.1f, 0.3f);

        transform.DOMoveY(pos.y + randomValue, 2.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }

    void Update() {
        // 上下にふわふわさせる
        //rb.MovePosition(new Vector2(pos.x, pos.y + Mathf.PingPong(Time.time, randomValue)));

        
    }
}
