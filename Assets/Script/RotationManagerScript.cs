using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class RotationManagerScript : MonoBehaviour
{
    public GameObject Rotator;
    public GameObject Goal;
    public float rotSpeed = 36f;
    public float rotSpeed2 = 36f;
    public float goalRotation = 0f;
    public float currentRotation = 0f;
    public float goalRotation2 = 0f;
    public float currentRotation2 = 0f;
    public int points;
    public GameObject container;
    public GameObject container2;
    public int lives;
    public GameObject Rotator2;
    public GameObject Goal2;
    public Coroutine move1;
    public Coroutine move2;
    public Coroutine rotate;
    public bool rot1;
    public bool rot2;
    public GameObject winScreen;
    public GameObject loseScreen;
    public GameObject rotateBar;
    public float barRotation;
    public bool barOut;
    public float barSpeed;
    public bool secondOut;

    // Start is called before the first frame update
    void Start()
    {
        barSpeed = 30f;
        barOut = false;
        secondOut = false;
        rotateBar = GameObject.Find("RotateBar");
        Rotator = GameObject.Find("Rotator");
        Goal = GameObject.Find("Goal Rotation");
        container = GameObject.Find("First");
        Rotator2 = GameObject.Find("Rotator2");
        Goal2 = GameObject.Find("Goal Rotation2");
        container2 = GameObject.Find("Second");
        rotSpeed = 54f;
        rotSpeed2 = -100f;
        points = 0;
        lives = 4;
        Flip();
        Flip2();
        rot1 = true;
        rot2 = true;
        container2.SetActive(false);
        winScreen = GameObject.Find("Win");
        loseScreen = GameObject.Find("Lose");
        winScreen.SetActive(false);
        loseScreen.SetActive(false);
        rotateBar.transform.eulerAngles = new Vector3(0, 90, 0);
        barRotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
        if(rot1){
            currentRotation = Rotator.transform.eulerAngles.z;
            Rotator.transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
        }
        if(rot2){
            currentRotation2 = Rotator2.transform.eulerAngles.z;
            Rotator2.transform.Rotate(0, 0, rotSpeed2 * Time.deltaTime);
        }
        if(points > 25){
            Win();
        }
        container.transform.eulerAngles = new Vector3(0, 0, 0);
        container2.transform.eulerAngles = new Vector3(0, 0, 0);
        if(Input.GetKeyDown(KeyCode.K)){
            BarRotate(30);
        }
        
        
        
        
        GameObject.Find("Lives").GetComponent<TextMeshProUGUI>().text = "Lives: " + lives;
        GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = "Score: " + points;
        if(Input.GetKeyDown(KeyCode.A)){
            if(Mathf.Abs(goalRotation - currentRotation) < 20f){
                points += 1;
                Flip();
            } else{
                lives -= 1;
                if(lives <= 0){
                    Lose();
                    Time.timeScale = 0;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.D)){
            if(Mathf.Abs(goalRotation2 - currentRotation2) < 20f){
                points += 1;
                Flip2();
            } else{
                lives -= 1;
                if(lives <= 0){
                    Lose();
                    Time.timeScale = 0;
                }
            }
        }

        
    }
    public void Flip(){
        goalRotation = Random.Range(0f, 360f);
        Goal.transform.eulerAngles = new Vector3(0, 0, goalRotation);
        rotSpeed *= -1.075f;
        if(points > 7){  
            if(secondOut == false){
                SecondOut();
            }
            
            if(rot2){
                rot1 = false;
                BarRotate(30);
            } else{
                rot2 = true;
                //MoveRandom();
                //MoveRandom2();
                BarRotate(30);
            }
        } else if(points > 3){
            BarRotate(30);
            //MoveRandom();
        } else{

        }
    }
    public void SecondOut(){
        secondOut = true;
        container2.SetActive(false);
        container2.transform.localPosition = new Vector3(container2.transform.localPosition.x - 200, container2.transform.localPosition.y, container2.transform.localPosition.z);
        container2.SetActive(true);
        StartCoroutine(MoveSecondOut());
    }
    public IEnumerator MoveSecondOut(){
        for(int i = 0; i < 100; i++){
            container2.transform.localPosition = new Vector3(container2.transform.localPosition.x + 2, container2.transform.localPosition.y, container2.transform.localPosition.z);
            yield return new WaitForSeconds(0.005f);
        }
    }
    public void MoveRandom(){
        if(move1 != null){
            StopCoroutine(move1);
        }
        Vector3 pos = new Vector3(Random.Range(-250f, 250f), Random.Range(-150f, 150f), 0) + new Vector3(427, 240, 0);
        move1 = StartCoroutine(MoveTo(pos));
    }
    public IEnumerator MoveTo(Vector3 pos){
        for(var i = 0; i < 50; i++){
            container.transform.position = Vector3.Lerp(container.transform.position, pos, Time.deltaTime * 7.5f);
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void Flip2(){
        goalRotation2 = Random.Range(0f, 360f);
        Goal2.transform.eulerAngles = new Vector3(0, 0, goalRotation2);
        rotSpeed2 *= -1.075f;
        if(points > 3){        
            if(rot1){
                rot2 = false;
                BarRotate(30);
            } else{
                rot1 = true;
                BarRotate(30);
                //MoveRandom();
                //MoveRandom2();
            }
        }
    }
    public void BarRotate(float degrees){
        if(barOut == false){
            barOut = true;
            StartCoroutine(UnleashBar());
        } else{
            Vector3 start = rotateBar.transform.eulerAngles;
            /*if(rotate != null){
                StopCoroutine(rotate);
            }*/
            rotate = StartCoroutine(Rot(start, barSpeed, Mathf.Abs(rotSpeed)));
            barSpeed += 10f;
        }
        
    }

    public IEnumerator Rot(Vector3 start, float degrees, float speed){
        for(var i = 0; i < 100; i++){
            rotateBar.transform.eulerAngles = rotateBar.transform.eulerAngles + new Vector3(0, 0, degrees / 100f);
            if(points % 5 == 0){
                GameObject.Find("RotateBar").GetComponent<RectTransform>().sizeDelta = new Vector2(GameObject.Find("RotateBar").GetComponent<RectTransform>().sizeDelta.x  + 0.25f, GameObject.Find("RotateBar").GetComponent<RectTransform>().sizeDelta.y);
            }
            yield return new WaitForSeconds(0.005f);
        }
    }
    public IEnumerator UnleashBar(){
        for(var i = 0; i < 150; i++){
            rotateBar.transform.eulerAngles = rotateBar.transform.eulerAngles - new Vector3(0, 0.6f, 0);
            yield return new WaitForSeconds(0.005f);
        }
    }
    public void MoveRandom2(){
        if(move2 != null){
            StopCoroutine(move2);
        }
        Vector3 pos = new Vector3(Random.Range(-250f, 250f), Random.Range(-150f, 150f), 0) + new Vector3(427, 240, 0);
        move2 = StartCoroutine(MoveTo2(pos));
    }
    public IEnumerator MoveTo2(Vector3 pos){
        for(var i = 0; i < 50; i++){
            container2.transform.position = Vector3.Lerp(container2.transform.position, pos, Time.deltaTime * 7.5f);
            yield return new WaitForSeconds(0.05f);
        }
    }
    public void Retry(){
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    void Lose(){
        loseScreen.SetActive(true);
    }
    void Win(){
        winScreen.SetActive(true);
    }
}
