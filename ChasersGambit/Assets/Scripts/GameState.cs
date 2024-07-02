using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using Controllers;
using UnityEngine.SceneManagement;

public static class GameState
{
    public static bool DidAnyHuntedExit = false;
    public static readonly string SessionId = GenerateSessionId(16);
    public static int NumberOfSwitches = 0;
    public static int numAggro = 0;
    public static string LevelName = SceneManager.GetActiveScene().name;
    public static int TryNumber = 1;
    public static int PowerUpNumber = 0;
    public static bool LastAttemptWasSuccess = false;

    public static float firstChase = -1.0f;
    public static float endTime = -1.0f;

    public static void ResetGameState()
    {
        NumberOfSwitches = 0;
        numAggro = 0;
        DidAnyHuntedExit = false;
        endTime = -1.0f;
        firstChase = -1.0f;
        PowerUpNumber = 0;
    }
    
    private static string GenerateSessionId(int length)
    {
        const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890-_";
        StringBuilder result = new StringBuilder(length);
        byte[] randomBytes = new byte[length];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        foreach (var byteValue in randomBytes)
        {
            result.Append(validChars[byteValue % validChars.Length]);
        }

        return result.ToString();
    }
    
    public static void DisableGo(GameObject gb)
    {
        MonoBehaviour[] scripts;
        scripts = gb.GetComponents<MonoBehaviour>();
        foreach(var script in scripts)
        {
            script.enabled = false;
        }

        HuntedController huntedController = gb.GetComponent<HuntedController>();
        huntedController.flashlight.enabled = false;
        //set Is Kinematic boolean to true for game object rigid body
        Rigidbody rb = gb.GetComponent<Rigidbody>();
        if (rb != null){
            rb.isKinematic = true;
        }
    }

    public static void EnableGo(GameObject gb)
    {
        MonoBehaviour[] scripts;
        scripts = gb.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = true;
        }

        HuntedController huntedController = gb.GetComponent<HuntedController>();
        huntedController.flashlight.enabled = true;
        Rigidbody rb = gb.GetComponent<Rigidbody>();
        if (rb != null){
            rb.isKinematic = false;
        }
    }
}
