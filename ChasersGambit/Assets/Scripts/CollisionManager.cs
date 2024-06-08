using System.Collections;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;

    public GameObject hunter;
    public Camera hunterCam;

    public GameObject hunted;
    public Camera huntedCam;

    public GameObject exit;
    public GameManager gameManager;

    public bool isHuntedActive = true;

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
        UpdatePlayerControl();
    }

    public void OnHuntedCollisionWithPowerUp(GameObject powerUp)
    {
        // Log a message to the console
        Debug.Log("Hunted collided with a PowerUp!");

        // Start the coroutine to switch roles for 5 seconds
        StartCoroutine(SwitchRolesForDuration(5.0f));

        // Optionally, destroy the power-up after collision
        Destroy(powerUp);
    }

    public void OnHuntedCollisionWithHunter(GameObject hunter)
    {
        // Log a message to the console
        Debug.Log("Hunted collided with hunter");

        //GAME OVER! - LOSS
        gameManager.LoseGame();
    }

    public void OnHuntedCollisionWithExit(GameObject exit)
    {
        // Log a message to the console
        Debug.Log("Hunted collided with exit");

        //GAME OVER! - WIN
        gameManager.WinGame();
    }

    void UpdatePlayerControl()
    {
        // Activate/Deactivate role controls and chase mechanic
        if (isHuntedActive)
        {
            hunted.GetComponent<HuntedController>().enabled = true;
            hunter.GetComponent<HunterController>().isChaseActive = true;
        }
        else
        {
            hunted.GetComponent<HuntedController>().enabled = false;
            hunter.GetComponent<HunterController>().isChaseActive = false;
        }
    }

    void UpdateCameraState()
    {
        // Activate/deactivate cameras
        huntedCam.gameObject.SetActive(isHuntedActive);
        hunterCam.gameObject.SetActive(!isHuntedActive);
    }

    IEnumerator SwitchRolesForDuration(float duration)
    {
        // Switch roles
        isHuntedActive = !isHuntedActive;
        UpdatePlayerControl();
        UpdateCameraState();

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Revert the roles back
        isHuntedActive = !isHuntedActive;
        UpdatePlayerControl();
        UpdateCameraState();
    }
}
