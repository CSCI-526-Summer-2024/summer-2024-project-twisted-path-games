using UnityEngine;
using UnityEngine.Networking;
using System.Text;
namespace DBManager
{
    public static class DBController
    {
        #if UNITY_WEBGL
            private static readonly string URL = "https://chasers-gambit-425703-default-rtdb.firebaseio.com/webgl/";
        #else
            private static readonly string URL = "https://chasers-gambit-425703-default-rtdb.firebaseio.com/";
        #endif

        public static void WriteToDB(string urlPath, string data)
        {
            // Create a new UnityWebRequest for the PUT method
            UnityWebRequest request = new UnityWebRequest(URL + urlPath, "PUT");
            
            byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.SetRequestHeader("Content-Type", "application/json");
            request.downloadHandler = new DownloadHandlerBuffer(); // Might not be required - check and remove this
            request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
            }
        }
    }
}
