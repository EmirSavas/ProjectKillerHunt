using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public CharacterMovement cm;
    
    public void Resume()
    {
        cm.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Scene_Lobby");
    }
}
