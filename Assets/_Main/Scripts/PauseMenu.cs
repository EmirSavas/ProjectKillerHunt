using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public CharacterMovement characterMovement;
    public CharacterMechanic characterMechanic;
    
    public void Resume()
    {
        characterMovement.enabled = true;
        characterMechanic.pauseGame = false;
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
