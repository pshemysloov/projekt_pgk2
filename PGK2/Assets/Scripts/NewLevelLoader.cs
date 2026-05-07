using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevelLoader : MonoBehaviour
{
    string newSceneName;
    Vector3 playerResetPosition;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void LoadNewScene(string newSceneName, Vector3 playerResetPosition) { 
        this.newSceneName = newSceneName;
        this.playerResetPosition = playerResetPosition;
        Debug.Log("Loading next level...");
        SceneManager.LoadScene(newSceneName);
    }



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != newSceneName) return;
        Debug.Log("New level loaded, resetting player position...");
        GameManager.instance.player.transform.position = playerResetPosition;

    }
}