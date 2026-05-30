using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryCameraSetter : MonoBehaviour
{
    public Canvas canvas;

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Camera cam = Camera.main;
        canvas.worldCamera = cam;
    }
}