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

    public Image fadeImage;
    public string nextSceneName = "Choose";

    private bool hasWon = false;
    // Start is called before the first frame update
    void Start()
    {
        fruitActive = false;
        gameSpeed = 0.7f;
        score = 0;
        Victory.SetActive(false);
        Defeat.SetActive(false);


        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

        #if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.L))
        {
            score = 14;
        }
        #endif

        if (!fruitActive && score < 15){
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
        gameSpeed *= 1.05f;
        score += 1;
    }
    public void Win()
    {
        if (hasWon) return;
        hasWon = true;
        GlobalUnlocksScript.completedFruitPiercer = true; // if you have a completion flag
        StartCoroutine(WinTransition());
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

    IEnumerator WinTransition()
    {
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);

            Color c = fadeImage.color;
            c.a = 1f;
            fadeImage.color = c;
        }

        yield return new WaitForSeconds(0.5f);

        SceneManager.LoadScene(nextSceneName);
    }
}
