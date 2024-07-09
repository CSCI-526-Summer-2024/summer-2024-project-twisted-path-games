using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    public GameObject playGameButtonObject;
    // Start is called before the first frame update
    void Start()
    {
        Button playGameButton = playGameButtonObject.GetComponent<Button>();
        playGameButton.onClick.AddListener(OnPlayButtonClick);
    }
    
    void OnPlayButtonClick()
    {
        GameState.LevelName = "level_picker";
        UnityEngine.SceneManagement.SceneManager.LoadScene("level picker");
    }

}
