using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
  public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
