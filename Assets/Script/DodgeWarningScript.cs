using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeWarningScript : MonoBehaviour
{
    public int lane;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(2.5f * lane, 4, 0);
    }
}
