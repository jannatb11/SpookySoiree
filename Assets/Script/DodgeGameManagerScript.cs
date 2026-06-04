using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DodgeGameManagerScript : MonoBehaviour
{
    public float obstacleSpawnTimer;
    public float obstacleSpawnTime;
    public GameObject obstaclePrefab;
    public float difficulty;
    public float timer;
    public GameObject victoryPanel;
    public GameObject defeatPanel;
    // Start is called before the first frame update
    void Start()
    {
        timer = 40f;
        obstacleSpawnTime = 2f;
        obstacleSpawnTimer = 0f;
        difficulty = 1f;
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        obstacleSpawnTimer += Time.deltaTime;
        if(obstacleSpawnTimer >= obstacleSpawnTime){
            obstacleSpawnTimer -= obstacleSpawnTime;
            SpawnObstacle();
        }
        obstacleSpawnTime = 2f - ((difficulty - 1) * 0.75f);
        obstacleSpawnTime = Mathf.Clamp(obstacleSpawnTime, 0.75f, 2f);
        timer -= Time.deltaTime;
        GameObject.Find("Timer").GetComponent<TextMeshProUGUI>().text = "" + Mathf.Round(timer);
        if(timer <= 0){
            Win();
        }
    }
    public void SpawnObstacle(){
        int lane = Random.Range(-1, 2);
        GameObject obstacle = Instantiate(obstaclePrefab);
        string type = (Random.Range(0, 2) == 1) ? "cup":"plate";
        obstacle.GetComponent<DodgeObstacleScript>().type = type;
        obstacle.GetComponent<DodgeObstacleScript>().speed = 10f * (1 +(difficulty-1)/2);
        obstacle.GetComponent<DodgeObstacleScript>().Initialize(lane);
        if(lane != 0 && timer < 20f && type == "cup" && Random.Range(0, 4) == 0){
            GameObject obstacle2 = Instantiate(obstaclePrefab);
            obstacle2.GetComponent<DodgeObstacleScript>().type = type;
            obstacle2.GetComponent<DodgeObstacleScript>().speed = 10f * (1 +(difficulty-1)/2);
            obstacle2.GetComponent<DodgeObstacleScript>().Initialize(-lane);
        }
        difficulty += 0.06f;
    }
    void Win(){
        GlobalUnlocksScript.completedDodgeMinigame = true;
        victoryPanel.SetActive(true);
        obstacleSpawnTime = 42e30f;
        obstacleSpawnTimer = -42e30f;
    }
    public void Lose(){
        if(victoryPanel.activeSelf){

        } else{
            defeatPanel.SetActive(true);
            timer = 370;
        }
    }
    public void Retry(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
