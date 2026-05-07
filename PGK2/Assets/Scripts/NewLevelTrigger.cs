using UnityEngine;
using UnityEngine.SceneManagement;

public class NewLevelTrigger : MonoBehaviour
{
    public string newSceneName;
    public Vector3 playerResetPosition;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Loading next level...");
            GameManager.instance.newLevelLoader.LoadNewScene(newSceneName, playerResetPosition);
        }
    }
}
