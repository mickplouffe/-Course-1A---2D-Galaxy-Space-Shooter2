using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool _isGameOver;
    [SerializeField] UIManager _uiManager;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && _isGameOver)
        {
            SceneManager.LoadScene(1); //Load Game
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && _isGameOver)
        {
            SceneManager.LoadScene(0); //Load Main Menu
        }

        if (Input.GetAxis("Cancel") == 1 && !_isGameOver)
        {
            _uiManager.ShowPauseMenu();
        }
    }
    public void GameOver()
    {
        _isGameOver = true;
    }
}
