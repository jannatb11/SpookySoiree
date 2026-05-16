using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FruitPiercerGMS : MonoBehaviour
{
    public GameObject fruit;
    public bool fruitActive;
    public float gameSpeed;
    public int score;
    public GameObject Victory;
    public GameObject Defeat;
    // Start is called before the first frame update
    void Start()
    {
        fruitActive = false;
        gameSpeed = 1f;
        score = 0;
        Victory.SetActive(false);
        Defeat.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(!fruitActive){
            fruitActive = true;
            GameObject newFruit = Instantiate(fruit);
            newFruit.GetComponent<FruitPiercerFruitScript>().delay = 1f / gameSpeed;
            newFruit.GetComponent<FruitPiercerFruitScript>().speed = gameSpeed;
        }
        GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = "" + score;
        if(score >= 15){
            Win();
        }
    }
    public void FruitHit(){
        fruitActive = false;
        gameSpeed *= 1.1f;
        score += 1;
    }
    public void Win(){
        Victory.SetActive(true);
        Time.timeScale = 0;
    }
    public void FruitLost(){
        fruitActive = false;
        Time.timeScale = 0;
        Defeat.SetActive(true);
    }
    public void Retry(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
