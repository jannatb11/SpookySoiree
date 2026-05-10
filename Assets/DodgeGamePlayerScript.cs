using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeGamePlayerScript : MonoBehaviour
{
    public int lane;
    // Start is called before the first frame update
    void Start()
    {
        lane = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)){
            if(lane > -1){
                lane -= 1;
            }
        }
        if(Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)){
            if(lane < 1){
                lane += 1;
            }
        }
        lane = Mathf.Clamp(lane, -1, 1);
        transform.position = new Vector3(2.5f * lane, transform.position.y, 0);
    }
}
