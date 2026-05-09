using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeObstacleScript : MonoBehaviour
{
    public string type;
    public float speed;
    public GameObject warning;
    // Start is called before the first frame update
    void Start()
    {
        switch (type){
            case "cup":
                int lane = Random.Range(-1, 2);
                GameObject warn = Instantiate(warning, transform.position, transform.rotation);
                warn.GetComponent<DodgeWarningScript>().lane = lane;
                transform.position = new Vector3(lane * 2.5f, 10, 0);
                
                break;
            case "plate":
                int xPos = (Random.Range(0, 2) == 0) ? -1:1;
                transform.position = new Vector3(xPos, 10, 0);
                transform.localScale = new Vector3(4, 1.5f, 1);
                GameObject warn1 = Instantiate(warning, transform.position, transform.rotation);
                warn1.GetComponent<DodgeWarningScript>().lane = 0;
                GameObject warn2 = Instantiate(warning, transform.position, transform.rotation);
                warn2.GetComponent<DodgeWarningScript>().lane = xPos;
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, Time.deltaTime * speed, 0);
        if(transform.position.y <= -7.5f){
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D other){
        Destroy(other.gameObject);
    }
    void OnCollisionEnter2D(Collision2D other){
        Debug.Log("BAD BAD BAD");
        Destroy(other.gameObject);
    }
}
