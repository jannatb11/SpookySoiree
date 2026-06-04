using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPiercerFruitScript : MonoBehaviour
{
    public List<Sprite> sprites;
    public float delay;
    public int moveDirection;
    public bool moving;
    public Rigidbody2D rb;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        delay = 1f;
        moving = false;
        moveDirection = 0;
        rb = GetComponent<Rigidbody2D>();
        GetComponent<SpriteRenderer>().sprite = sprites[Random.Range(0, sprites.Count)];
    }

    // Update is called once per frame
    void Update()
    {
        if(delay <= 0f && !moving){
            moving = true;
            moveDirection = Random.Range(1, 5);
        }else{
            delay -= Time.deltaTime;
        }
        if(moving){
            switch(moveDirection){
                case 1:
                    rb.AddForce(transform.up * speed);
                    break;
                case 2:
                    rb.AddForce(transform.right * speed);
                    break;
                case 3:
                    rb.AddForce(transform.up * -1 * speed);
                    break;
                case 4:
                    rb.AddForce(transform.right * -1 * speed);
                    break;
                default:
                    break;
                
            }
        }
        if(Vector3.Distance(transform.position, new Vector3(0, 0, transform.position.z)) >= 4.5f){
            GameObject.Find("GameManager").GetComponent<FruitPiercerGMS>().FruitLost();
            Destroy(gameObject);
        }
    }
    void OnCollisionEnter2D(Collision2D other){
        GameObject.Find("GameManager").GetComponent<FruitPiercerGMS>().FruitHit();
        Destroy(gameObject);
    }
}
