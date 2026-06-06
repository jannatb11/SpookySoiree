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

        if (Input.GetKeyDown(KeyCode.W))
        {
            rotation = 2; // Top
            HideInstructions();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            rotation = 1; // Right
            HideInstructions();
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            rotation = 0; // Bottom
            HideInstructions();
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            rotation = 3; // Left
            HideInstructions();
        }
    }

    void HideInstructions()
    {
        if (GameObject.Find("Instructions") != null)
        {
            GameObject.Find("Instructions").SetActive(false);
        }
    }
}
