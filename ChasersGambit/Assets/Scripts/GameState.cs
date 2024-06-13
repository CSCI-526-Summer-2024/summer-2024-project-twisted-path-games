using UnityEngine;
public static class GameState
{
    public static bool DidAnyHuntedExit = false;
    
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
