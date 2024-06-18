using UnityEngine;
namespace DBManager
{
    public static class AnalyticManager
    {
        public static void WriteNumberOfSwitches()
        {
            string urlPath = $"sessions/{GameState.SessionId}/{GameState.LevelNumber}/{GameState.TryNumber++}.json";
            NumberOfSwitches data = new NumberOfSwitches()
            {
                numberOfSwitches = GameState.NumberOfSwitches,
                wasSuccessfull = GameState.LastAttemptWasSuccess
            };
            string dataString = JsonUtility.ToJson(data);
            DBController.WriteToDB(urlPath, dataString);  
        } 
    }
}