using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RotatorScript : MonoBehaviour
{
    public GameObject Rotator;
    public GameObject Goal;
    public float rotSpeed = 36f;
    public float goalRotation = 0f;
    public float currentRotation = 0f;
    public int points;
    public GameObject container;
    public int lives;
    // Start is called before the first frame update
    void Start()
    {
        Rotator = GameObject.Find("Rotator");
        Goal = GameObject.Find("Goal Rotation");
        container = GameObject.Find("First");
        rotSpeed = 36f;
        points = 0;
        lives = 3;
        Flip();
    }

    // Update is called once per frame
    void Update()
    {
        currentRotation = Rotator.transform.eulerAngles.z;
        Rotator.transform.Rotate(0, 0, rotSpeed * Time.deltaTime);
        GameObject.Find("Lives").GetComponent<TextMeshProUGUI>().text = "Lives: " + lives;
        GameObject.Find("Score").GetComponent<TextMeshProUGUI>().text = "Score: " + points;
        //print(currentRotation);
        if(Input.GetKeyDown(KeyCode.Space)){
            if(Mathf.Abs(goalRotation - currentRotation) < 20f){
                print("GOOD");
                points += 1;
                Flip();
            } else{
                print("BAD BAD BAD");
                lives -= 1;
                if(lives <= 0){
                    Time.timeScale = 0;
                }
            }
        }
        if(Input.GetKeyDown(KeyCode.K)){
            MoveRandom();
        }
        
    }
    public void Flip(){
        goalRotation = Random.Range(0f, 360f);
        Goal.transform.eulerAngles = new Vector3(0, 0, goalRotation);
        rotSpeed *= -1.1f;
        if(points > 5){
            MoveRandom();
        }
    }
    public void MoveRandom(){
        StopAllCoroutines();
        print(container.transform.position);
        Vector3 pos = new Vector3(Random.Range(-250f, 250f), Random.Range(-150f, 150f), 0) + new Vector3(427, 240, 0);
        StartCoroutine(MoveTo(pos));
    }
    public IEnumerator MoveTo(Vector3 pos){
        for(var i = 0; i < 50; i++){
            container.transform.position = Vector3.Lerp(container.transform.position, pos, Time.deltaTime * 7.5f);
            yield return new WaitForSeconds(0.05f);
        }
    }
}
