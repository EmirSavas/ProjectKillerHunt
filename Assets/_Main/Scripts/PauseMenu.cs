using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public CharacterMovement characterMovement;
    public CharacterMechanic characterMechanic;
    public Dropdown dropDown;
    public Scrollbar scrollBar;
    
    public void Resume()
    {
        characterMovement.enabled = true;
        characterMechanic.pauseGame = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    public void OptionsQualitySettings()
    {
        QualitySettings.SetQualityLevel(dropDown.value, true);
    }

    public void VolumeSettings()
    {
        AudioListener.volume = scrollBar.value;
    }

    public void MuteSound()
    {
        AudioListener.volume = 0;
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
