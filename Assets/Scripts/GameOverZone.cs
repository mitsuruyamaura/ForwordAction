using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    //[HideInInspector]
    //public GameManager gameManager;


    [SerializeField]
    private AudioManager audioManager;

    private void OnTriggerEnter2D(Collider2D col) {

        if(col.TryGetComponent(out PlayerController player)) {
            //if (col.gameObject.tag == "Player") {
            //    col.gameObject.GetComponent<PlayerController>().GameOver();

            //gameManager.GameOver();

            player.GameOver();

            StartCoroutine(audioManager.PlayBGM(3));

            Debug.Log("Game Over");
        }
    }
}
