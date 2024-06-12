using System.Collections;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public static CollisionManager Instance;

    public GameObject hunter;
    public Camera hunterCam;

    public GameObject hunted1;
    public Camera huntedCam1;
    
    public GameObject hunted2;
    public Camera huntedCam2;

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
        UpdatePlayerControl(hunted1);
    }

    public void OnHuntedCollisionWithPowerUp(GameObject powerUp, GameObject collidingHunted)
    {
        // Log a message to the console
        Debug.Log("Hunted collided with a PowerUp!");

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

    public void OnHuntedCollisionWithExit()
    {
        // Log a message to the console
        Debug.Log("Hunted collided with exit");

        //GAME OVER! - WIN
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
        // Switch roles
        isHuntedActive = !isHuntedActive;
        UpdatePlayerControl(collidingHunted);
        UpdateCameraState(collidingHunted);

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Revert the roles back
        isHuntedActive = !isHuntedActive;
        UpdatePlayerControl(collidingHunted);
        UpdateCameraState(collidingHunted);
    }
}
