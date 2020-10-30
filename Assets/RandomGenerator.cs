using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour
{
    public GameObject[] generateObjs;

    public Transform generateTran;

    public float waitTime;

    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= waitTime) {
            timer = 0;
            Generate();
        }   
    }

    private void Generate() {
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
        float randomPos = Random.Range(-4.0f, 4.0f);

        obj.transform.position = new Vector2(obj.transform.position.x, obj.transform.position.y + randomPos);
    }
}
