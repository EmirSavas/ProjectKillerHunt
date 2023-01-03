using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private NetworkManagerLobby networkManager;

    [SerializeField] private GameObject landingPagePanel;
    
    public TMP_Dropdown dropDown;
    
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
