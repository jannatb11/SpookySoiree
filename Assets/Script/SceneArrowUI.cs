using UnityEngine;

public class SceneArrowUI : MonoBehaviour
{
    public string sceneName;
    public TravelScript travelScript;

    void Update()
    {
        if (travelScript == null) return;

        gameObject.SetActive(travelScript.CanShowArrow(sceneName));
    }

    public void OnClick()
    {
        travelScript.Load(sceneName);
    }
}