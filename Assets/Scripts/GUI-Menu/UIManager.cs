using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UIManager : MonoBehaviour
{
    [SerializeField] GameManager _gameManager;
    [SerializeField] Text _scoreText;
    [SerializeField] Text _gameOverText;
    [SerializeField] Text _restartInfo_Text;
    [SerializeField] Canvas _pauseMenu;
    [SerializeField] Sprite[] _liveSprites;
    [SerializeField] Image _livesImg;

    void Start()
    {
        _scoreText.text = "Score: " + 0;
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore;
    }

    public void UpdateLives(int currentLives)
    {
        Debug.Log(currentLives);
        _livesImg.sprite = _liveSprites[currentLives];
    }

    public void GameOver()
    {
        _gameManager.GameOver();
        _gameOverText.gameObject.SetActive(true);
        _restartInfo_Text.gameObject.SetActive(true);

    }

    public void ShowPauseMenu()
    {
        _pauseMenu.gameObject.SetActive(true);
        Time.timeScale = 0.000001f; //Was working at 0 in another personal project. Need Further Test.
    }

    public void HidePauseMenu()
    {
        Time.timeScale = 1;
        _pauseMenu.gameObject.SetActive(false);
        
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1); //Load Scene 1 = Game
    }

    public void Quit()
    {

#if UNITY_EDITOR   
        UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
    }

}