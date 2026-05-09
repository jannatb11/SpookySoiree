using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DodgeGameManagerScript : MonoBehaviour
{
    public float obstacleSpawnTimer;
    public float obstacleSpawnTime;
    public GameObject obstaclePrefab;
    public float difficulty;
    public float timer;
    public GameObject victoryPanel;
    // Start is called before the first frame update
    void Start()
    {
        timer = 60f;
        obstacleSpawnTime = 2f;
        obstacleSpawnTimer = 0f;
        difficulty = 1f;
        victoryPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        obstacleSpawnTimer += Time.deltaTime;
        if(obstacleSpawnTimer >= obstacleSpawnTime){
            obstacleSpawnTimer -= obstacleSpawnTime;
            SpawnObstacle();
        }
        obstacleSpawnTime = 2f - ((difficulty - 1) * 0.5f);
        obstacleSpawnTime = Mathf.Clamp(obstacleSpawnTime, 0.5f, 2f);
        timer -= Time.deltaTime;
        GameObject.Find("Timer").GetComponent<TextMeshProUGUI>().text = "" + Mathf.Round(timer);
        if(timer <= 0){
            Win();
        }
    }
    public void SpawnObstacle(){
        GameObject obstacle = Instantiate(obstaclePrefab);
        obstacle.GetComponent<DodgeObstacleScript>().type = (Random.Range(0, 2) == 1) ? "cup":"plate";
        obstacle.GetComponent<DodgeObstacleScript>().speed = 5f * difficulty;
        difficulty += 0.075f;
    }
    void Win(){
        victoryPanel.SetActive(true);
    }
}
