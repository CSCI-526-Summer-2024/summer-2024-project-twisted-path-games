using System.Security.Cryptography;
using System.Text;
using UnityEngine;
public static class GameState
{
    public static bool DidAnyHuntedExit = false;
    public static readonly string SessionId = GenerateSessionId(16);
    public static int NumberOfSwitches = 0;
    public static int LevelNumber = 1;
    public static int TryNumber = 1;
    public static bool LastAttemptWasSuccess = false;

    public static void ResetGameState()
    {
        TryNumber = 1;
        NumberOfSwitches = 0;
        DidAnyHuntedExit = false;
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
    }

    public static void EnableGo(GameObject gb)
    {
        MonoBehaviour[] scripts;
        scripts = gb.GetComponents<MonoBehaviour>();
        foreach (var script in scripts)
        {
            script.enabled = true;
        }
    }
}
