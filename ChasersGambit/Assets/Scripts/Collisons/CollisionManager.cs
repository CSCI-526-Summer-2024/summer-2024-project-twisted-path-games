using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Controllers;
using DBManager;
using TMPro;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;

    public GameObject hunter;
    public Camera hunterCam;
    private HunterController hunterController;

    public GameObject hunted1;
    public Camera huntedCam1;
    public GameObject cameraHolder1;
    
    public GameObject hunted2;
    public Camera huntedCam2;
    public GameObject cameraHolder2;

    public Light mazeLights;
    
    public GameManager gameManager;

    public bool isHuntedActive = true;
    public List<Vector3> hunterPos;

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
        // Log a message to the console
        Debug.Log("Hunted collided with a PowerUp!");
        GameState.PowerUpNumber++;

        // Start the coroutine to switch roles for 5 seconds
        StartCoroutine(SwitchRolesForDuration(5.0f, collidingHunted));

        // Optionally, destroy the power-up after collision
        Destroy(powerUp);
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
        if (GameState.DidAnyHuntedExit)
        {
            OnBothHuntedExitMaze();
        }
        else
        {
            DisableHuntedAfterExiting(huntedThatExited);
            GameState.DidAnyHuntedExit = true;
            hunterController = hunter.GetComponent<HunterController>();

            if (hunterController == null)
            {
                Debug.LogError("HunterController component not found on otherHunted");
                return;
            }

            hunterController.UpdateTarget(otherHunted);
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
            hunter.GetComponent<HunterController>().isChaseActive = true;
        }
        else
        {
            if (collidingHunted.CompareTag("Hunted1"))
            {
                hunted1.GetComponent<HuntedController>().enabled = false;
            }
            else
            {
                hunted2.GetComponent<HuntedController>().enabled = false;
            }
            hunter.GetComponent<HunterController>().isChaseActive = false;
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
        
        countdown.gameObject.SetActive(true);
        int countdownText = 5;
        countdown.text = countdownText.ToString();
        hunterPos = new List<Vector3>();
        
        // Wait for the specified duration
        for (int i = 0; i < 10; i++)
        {
            if (i % 2 == 0)
            {
                countdown.text = countdownText--.ToString();  
            }
            yield return new WaitForSeconds(duration/10);
            hunterPos.Add(hunter.transform.position);
        }
        
        countdown.gameObject.SetActive(false);
        AnalyticManager.WritePositions(hunterPos);
        
        // Return to dark lighting for first person view
        mazeLights.enabled = false;

        // Revert the roles back
        isHuntedActive = !isHuntedActive;
        UpdatePlayerControl(collidingHunted);
        UpdateCameraState(collidingHunted);
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
