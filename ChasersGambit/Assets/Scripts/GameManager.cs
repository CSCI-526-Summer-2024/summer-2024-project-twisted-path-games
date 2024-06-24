using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject winUI;
    public GameObject winL1;
    public GameObject winL2;
    public GameObject loseUI;
    public GameObject playAgainButtonObject;
    public GameObject playNextLevelButtonObject;
    public GameObject goBackButtonObject;
   
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
        Debug.Log("game starting");
        if (playAgainButtonObject == null)
        {
            Debug.LogError("ReplayButtonObject is not assigned.");
            return;
        }

        Button playAgainButton = playAgainButtonObject.GetComponent<Button>();
        playAgainButton.onClick.AddListener(OnReplayButtonClick);
        
        Button playNextLevelButton = playNextLevelButtonObject.GetComponent<Button>();
        playNextLevelButton.onClick.AddListener(OnNextLevelButtonClick);
        
        Button goBackButton = goBackButtonObject.GetComponent<Button>();
        goBackButton.onClick.AddListener(OnGoToLevelPickerButtonClick);
    }

    public void WinGame()
    {
        Time.timeScale = 0f;
        GameState.endTime = Time.time;
        GameState.LastAttemptWasSuccess = true;
        if (GameState.LevelNumber == 1)
        {
            winL1.SetActive(true);   
            playNextLevelButtonObject.SetActive(true);
        }
        else if (GameState.LevelNumber == 2)
        {
            winL2.SetActive(true);   
            playNextLevelButtonObject.SetActive(true);
        }
        else if (GameState.LevelNumber == 3)
        {
            winUI.SetActive(true);   
        }
        
        goBackButtonObject.SetActive(true);
        playAgainButtonObject.SetActive(true);
        
        DBManager.AnalyticManager.WriteAggro();
        DBManager.AnalyticManager.WriteNumberOfSwitches();
        GameState.ResetGameState();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoseGame()
    {
        GameState.endTime = Time.time;
        Time.timeScale = 0f;

        GameState.LastAttemptWasSuccess = false;
        loseUI.SetActive(true);
        playAgainButtonObject.SetActive(true);
        
        DBManager.AnalyticManager.WriteAggro();
        DBManager.AnalyticManager.WriteNumberOfSwitches();
        GameState.ResetGameState();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnNextLevelButtonClick()
    {
        if (GameState.LevelNumber == 1)
        {
            GameState.TryNumber = 1;
            GameState.LevelNumber = 2;
            UnityEngine.SceneManagement.SceneManager.LoadScene("level2");
        }
        else if (GameState.LevelNumber == 2)
        {
            GameState.TryNumber = 1;
            GameState.LevelNumber = 3;
            UnityEngine.SceneManagement.SceneManager.LoadScene("level3");
        }
    }
    void OnReplayButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    void OnGoToLevelPickerButtonClick()
    {
        GameState.LevelNumber = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("level picker");
    }
}
