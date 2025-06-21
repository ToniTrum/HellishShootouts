using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _highScoresPanel;

    private void Start()
    {
        _mainMenuPanel.SetActive(true);
        _highScoresPanel.SetActive(false);
    }
    
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowHighScores()
    {
        _mainMenuPanel.SetActive(false);
        _highScoresPanel.SetActive(true);
    }

    public void HideHighScores()
    {
        _highScoresPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
    }
} 