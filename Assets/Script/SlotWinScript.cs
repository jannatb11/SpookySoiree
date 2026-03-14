using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotWinScript : MonoBehaviour
{
    public bool correct;
    public Color desiredColor;
    // Start is called before the first frame update
    void Start()
    {
        correct = false;
    }

    // Update is called once per frame
    void Update()
    {
        correct = (gameObject.GetComponent<Image>().color == desiredColor);
    }
}
