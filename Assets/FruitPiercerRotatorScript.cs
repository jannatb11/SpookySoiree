using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FruitPiercerRotatorScript : MonoBehaviour
{
    public float rotation = 0;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles = new Vector3(0, 0, rotation * 90);
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow)){
            rotation += Input.GetAxisRaw("Horizontal");
        }
    }
}
