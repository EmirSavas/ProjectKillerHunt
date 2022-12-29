using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager = null;

    [SerializeField] private GameObject landingPagePanel = null;
    
    public Dropdown dropDown;
    
    public Scrollbar scrollBar;

    public void HostLobby()
    {
        networkManager.StartHost();
        
        landingPagePanel.SetActive(false);
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
}
