using UnityEngine;
namespace DBManager
{
    public static class AnalyticManager
    {
        private static int SwitchIncrementer = 1;
        public static void WriteNumberOfSwitches()
        {
            string urlPath = $"sessions/{GameState.SessionId}/{GameState.LevelNumber}/{SwitchIncrementer++}.json";
            NumberOfSwitches data = new NumberOfSwitches()
            {
                numberOfSwitches = GameState.NumberOfSwitches
            };
            string dataString = JsonUtility.ToJson(data);
            DBController.WriteToDB(urlPath, dataString);  
        } 
    }
}