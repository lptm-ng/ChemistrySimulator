using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject infoPanel;
    
    public void PlayGame()
    {
        SceneManager.LoadScene("Test Chemie"); // TODO: real scene loaden
    }

    public void OpenInfo()
    {
        infoPanel.SetActive(true);
    }

    public void CloseInfo()
    {
        infoPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
