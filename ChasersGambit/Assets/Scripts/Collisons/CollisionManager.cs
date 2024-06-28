using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using DBManager;
using TMPro;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;

    // TODO update hunter and hunter controllers to array
    public GameObject hunter1;
    public GameObject hunter2;
    public Camera hunterCam;
    private HunterController hunterController1;
    private HunterController hunterController2;
    public GameObject hunted1PlayIcon;

    public GameObject hunted1;
    public Camera huntedCam1;
    public GameObject cameraHolder1;
    public GameObject hunted1ExitIcon;
    public GameObject hunted2PlayIcon;
    
    
    public GameObject hunted2;
    public Camera huntedCam2;
    public GameObject cameraHolder2;
    public GameObject hunted2ExitIcon;
    


    public Light mazeLights;
    
    public GameManager gameManager;

    public bool isHuntedActive = true;
    private List<Vector3> hunter1Pos;
    private List<Vector3> hunter2Pos;

    public TextMeshProUGUI countdown;

    void Awake()
    {
        // Ensure there is only one instance of the manager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        UpdatePlayerControl(hunted1);
    }

    public void OnHuntedCollisionWithPowerUp(GameObject powerUp, GameObject collidingHunted)
    {
        Debug.Log("Hunted collided with a PowerUp!");
        GameState.PowerUpNumber++;

        // Start the coroutine to switch roles for 5 seconds
        StartCoroutine(SwitchRolesForDuration(5.0f, collidingHunted));

        Destroy(powerUp);
    }
    
    private void EnableArrowsAroundHunters()
    {
        List<GameObject> hunterControls = GetAllInactiveGameObjectsWithTag("HunterControls");

        foreach (GameObject hunterControl in hunterControls)
        {
            hunterControl.SetActive(true);
        }
    }

    private void DisableArrowsAroundHunters()
    {
        List<GameObject> hunterControls = GetAllInactiveGameObjectsWithTag("HunterControls");

        foreach (GameObject hunterControl in hunterControls)
        {
            hunterControl.SetActive(false);
        }
    }

    List<GameObject> GetAllInactiveGameObjectsWithTag(string tag)
    {
        List<GameObject> gameObjectsWithTag = new List<GameObject>();
        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject go in allGameObjects)
        {
            if (go.CompareTag(tag))
            {
                gameObjectsWithTag.Add(go);
            }
        }

        return gameObjectsWithTag;
    }

    public void OnHunterCollisionWithLevelDoor(string tag)
    {
        switch (tag)
        {
            case "Tutorial":
                GameState.LevelNumber = 1;
                UnityEngine.SceneManagement.SceneManager.LoadScene("powerup-tutorial");
                break;
            case "Level1":
                GameState.LevelNumber = 2;
                UnityEngine.SceneManagement.SceneManager.LoadScene("level2");
                break;
            case "Level2":
                GameState.LevelNumber = 3;
                UnityEngine.SceneManagement.SceneManager.LoadScene("level3");
                break;
            default:
                GameState.LevelNumber = 0;
                UnityEngine.SceneManagement.SceneManager.LoadScene("level picker");
                break;
        }
    }

    public void OnHuntedCollisionWithHunter()
    {
        // Log a message to the console
        Debug.Log("Hunted collided with hunter");

        //GAME OVER! - LOSS
        gameManager.LoseGame();
    }

    public void OnHuntedCollisionWithExit(GameObject huntedThatExited, GameObject otherHunted)
    {
        if (huntedThatExited.CompareTag("Hunted1"))
        { 
            hunted1ExitIcon.SetActive(true);
            hunted1PlayIcon.SetActive(false); 
        }
        else
        {
            hunted2ExitIcon.SetActive(true);
            hunted2PlayIcon.SetActive(false); 
        }
        if (GameState.DidAnyHuntedExit)
        {
            OnBothHuntedExitMaze();
        }
        else
        {
            DisableHuntedAfterExiting(huntedThatExited);
            GameState.DidAnyHuntedExit = true;
            hunterController1 = hunter1.GetComponent<HunterController>();
            hunterController2 = hunter2.GetComponent<HunterController>();


            if (hunterController1 == null || hunterController2 == null)
            {
                Debug.LogError("HunterController component not found on otherHunted");
                return;
            }

            hunterController1.UpdateTarget(otherHunted);
            hunterController2.UpdateTarget(otherHunted);
        }
    }
    
    public void OnBothHuntedExitMaze()
    {
        gameManager.WinGame();
    }

    void UpdatePlayerControl(GameObject collidingHunted)
    {
        // Activate/Deactivate role controls and chase mechanic
        if (isHuntedActive)
        {
            if (collidingHunted.CompareTag("Hunted1"))
            {
                hunted1.GetComponent<HuntedController>().enabled = true;
            }
            else
            {
                hunted2.GetComponent<HuntedController>().enabled = true;
            }
            hunter1.GetComponent<HunterController>().isChaseActive = true;
            if (hunter2){
                hunter2.GetComponent<HunterController>().isChaseActive = true;
            }
        }
        else
        {
            if (collidingHunted.CompareTag("Hunted1"))
            {
                hunted1.GetComponent<HuntedController>().enabled = false;
            }
            else if (hunter2)
            {
                hunted2.GetComponent<HuntedController>().enabled = false;
            }
            hunter1.GetComponent<HunterController>().isChaseActive = false;
            if (hunter2){
                hunter2.GetComponent<HunterController>().isChaseActive = false;
            }
            //where you decide which hunter to give control to first. can change to a different method
            //proximity...last controlled...etc
            hunter1.GetComponent<HunterController>().controlled = true;
            if (hunter2){
                hunter2.GetComponent<HunterController>().controlled = false;
            }
        }
    }

    void UpdateCameraState(GameObject collidingHunted)
    {
        // Activate/deactivate cameras
        if (collidingHunted.CompareTag("Hunted1"))
        {
            huntedCam1.gameObject.SetActive(isHuntedActive);
        }
        else
        {
            huntedCam2.gameObject.SetActive(isHuntedActive);
        }
        hunterCam.gameObject.SetActive(!isHuntedActive);
    }

    IEnumerator SwitchRolesForDuration(float duration, GameObject collidingHunted)
    {        
        if (mazeLights == null)
        {
            Debug.LogError("No Light component found on this GameObject.");
        }

        // Brighten the map so the player can see
        mazeLights.enabled = true;

        // Switch roles
        isHuntedActive = !isHuntedActive;
        UpdatePlayerControl(collidingHunted);
        UpdateCameraState(collidingHunted);
        //Enable the arrows around hunters
        EnableArrowsAroundHunters();
        
        countdown.gameObject.SetActive(true);
        int countdownText = 5;
        countdown.text = countdownText.ToString();
        hunter1Pos = new List<Vector3>();
        hunter2Pos = new List<Vector3>();
        
        // Wait for the specified duration
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                countdown.text = countdownText--.ToString();  
            }
            yield return new WaitForSeconds(duration/10);
            hunter1Pos.Add(hunter1.transform.position);
            hunter2Pos.Add(hunter2.transform.position);
        }
        
        countdown.gameObject.SetActive(false);
        AnalyticManager.WritePositions(hunter1Pos, hunter2Pos);
        
        // Return to dark lighting for first person view
        mazeLights.enabled = false;

        // Revert the roles back
        isHuntedActive = !isHuntedActive;
        UpdatePlayerControl(collidingHunted);
        UpdateCameraState(collidingHunted);
        
        //Disable the arrows around hunters
        DisableArrowsAroundHunters();
    }

    void DisableHuntedAfterExiting(GameObject huntedThatExited)
    {
        if (huntedThatExited.CompareTag("Hunted1"))
        {
            GameState.EnableGo(hunted2);
            cameraHolder1.SetActive(false);
            huntedCam2.gameObject.SetActive(true);
        }
        else
        {
            GameState.EnableGo(hunted1);
            cameraHolder2.SetActive(false);
            huntedCam1.gameObject.SetActive(true);
        }
        huntedThatExited.SetActive(false);
    }
}
