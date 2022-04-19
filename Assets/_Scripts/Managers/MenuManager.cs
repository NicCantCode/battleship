using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }

    public void RestartGameScene()
    {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(0);
    }
}
