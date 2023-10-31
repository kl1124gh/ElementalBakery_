using UnityEngine;
using UnityEngine.SceneManagement;

public class nextLevelButton : MonoBehaviour
{
    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
