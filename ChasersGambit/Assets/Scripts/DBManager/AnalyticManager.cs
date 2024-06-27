using System.Collections.Generic;
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
            float timeSinceAggro;
            if (GameState.firstChase < 0){
                timeSinceAggro = 0;
            }
            else{
                timeSinceAggro = GameState.endTime - GameState.firstChase;
            }
            NumberofAggro data = new NumberofAggro()
            {
                numAggro = GameState.numAggro,
                wasSuccessful = GameState.LastAttemptWasSuccess,
                timeSinceFirstAggro = timeSinceAggro
            };
            string dataString = JsonUtility.ToJson(data);
            DBController.WriteToDB(urlPath, dataString);  
        }
        
        public static void WritePositions(List<Vector3> hunter1Positions, List<Vector3> hunter2Positions)
        {
            string urlPath = $"sessions/{GameState.SessionId}/{GameState.LevelNumber}/{GameState.TryNumber}/hunterPositions/{GameState.PowerUpNumber}.json";
            HunterMovement data = new HunterMovement()
            {
                hunter1Positions = hunter1Positions,
                hunter2Positions = hunter2Positions,
            };
            string dataString = JsonUtility.ToJson(data);
            DBController.WriteToDB(urlPath, dataString);
        }
    }
}