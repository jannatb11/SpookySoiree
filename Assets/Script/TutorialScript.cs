using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    public GameObject SceneButton;
    public GameObject ContinueArrow;
    public GameObject popupUI;

    public void Awake()
    {
        if(ContinueArrow != null){
            ContinueArrow.SetActive(true);
        }
        popupUI.SetActive(false);
        SceneButton.SetActive(false);
    }

    /*public void ShowContinueArrow()
    {
        ContinueArrow.SetActive(true);
        popupUI.SetActive(false);
        SceneButton.SetActive(false);

    }*/

    public void ContinueClicked()
    {
        ContinueArrow.SetActive(false);
        popupUI.SetActive(true);
        SceneButton.SetActive(true);
    }
}