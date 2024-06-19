using Controllers;
using UnityEngine;
namespace DBManager
{
    public static class AnalyticManager
    {
        public static void WriteNumberOfSwitches()
        {
            string urlPath = $"sessions/{GameState.SessionId}/{GameState.LevelNumber}/{GameState.TryNumber++}/switches.json";
            NumberOfSwitches data = new NumberOfSwitches()
            {
                numberOfSwitches = GameState.NumberOfSwitches,
                wasSuccessfull = GameState.LastAttemptWasSuccess
            };
            string dataString = JsonUtility.ToJson(data);
            DBController.WriteToDB(urlPath, dataString);  
        } 
        public static void WriteAggro()
        {
            string urlPath = $"sessions/{GameState.SessionId}/{GameState.LevelNumber}/{GameState.TryNumber}/aggro.json";
            NumberofAggro data = new NumberofAggro()
            {
                numAggro = GameState.numAggro,
                wasSuccessful = GameState.LastAttemptWasSuccess,
                duration = GameState.endTime - GameState.firstChase
            };
            string dataString = JsonUtility.ToJson(data);
            DBController.WriteToDB(urlPath, dataString);  
        } 
    }
}