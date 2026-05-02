using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPieceScript : MonoBehaviour
{
    public float rotation;
    public float rotationStep;
    public float maxRotation;
    public bool rotating;
    // Start is called before the first frame update
    void Start()
    {
        rotating = false;
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, rotation);
        
    }
    public void Rotate(){
        if(GameObject.Find("GameCanvas").GetComponent<Connect4GameScript>().instructions < 1){
            GameObject.Find("GameCanvas").GetComponent<Connect4GameScript>().Instructions();
        }
        if(!rotating){
            if(rotation == 360){
                rotation = 0;
            }
            float rotGoal;
            if(rotation != maxRotation){
                rotGoal = rotation + rotationStep;
            } else{
                rotGoal = 0;
            }
            StartCoroutine(RotateTo(rotGoal));
        }
    }
    public IEnumerator RotateTo(float goalRot){
        rotating = true;
        float startRot = rotation;
        float num = 0f;
        while(rotation != goalRot){
            rotation = Mathf.Lerp(startRot, goalRot, num);
            num += Time.deltaTime * 5f;
            yield return new WaitForSeconds(0.01f);
        }
        rotating = false;
        
    }
}
