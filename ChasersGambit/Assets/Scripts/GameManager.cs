using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject winUI;
    public GameObject loseUI;
    public GameObject playAgainButtonObject;
   
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
    }

    public void WinGame()
    {
        Time.timeScale = 0f;

        winUI.SetActive(true);
        playAgainButtonObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void LoseGame()
    {
        Time.timeScale = 0f;

        loseUI.SetActive(true);
        playAgainButtonObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void OnReplayButtonClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        Debug.Log("Button Clicked");
    }
}
