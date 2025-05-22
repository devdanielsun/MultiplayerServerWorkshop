using UnityEngine;
using UnityEngine.Networking;

public static class LoggingHelper
{
    public static void LogApiError(string title, UnityWebRequest request)
    {
        string errorMsg = request.error;
        // Try to get the response body for more details (e.g., 400 Bad Request)
        string responseText = request.downloadHandler != null ? request.downloadHandler.text : "";
        if (!string.IsNullOrEmpty(responseText))
        {
            errorMsg += $" - Server response: {responseText}";
        }
        Debug.LogError($"{title}: ({request.responseCode}) - {errorMsg}");
    }
}
