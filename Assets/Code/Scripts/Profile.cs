using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RehabiliTEA
{
    public class Profile
    {   
        private class Response { public bool isError = false; public string result = ""; }
        private class DifficultyResponse { public string difficulty = ""; }

        private string          currentTask     = "";
        private bool            hasInternet     = true;
        private int             id              = 1;
        private Difficulty      taskDifficulty  = Difficulty.Hard;
        private const string    baseURL         = "http://localhost";
        private static Profile  UserProfile     = null;

        private Profile()
        {
            string filePath = Application.persistentDataPath + "/user.txt";
            string internet = GetRequestAt(baseURL + "/ping");

            if (File.Exists(filePath))
            {
                id = int.Parse(File.ReadAllLines(filePath)[0]);
            }

            hasInternet = !internet.Equals("error");
        }

        ~Profile()
        {
            string filePath = Application.persistentDataPath + "/user.txt";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            var file = File.CreateText(filePath);
            file.Write(id.ToString());
            file.Close();
        }

        private string GetRequestAt(string endpoint)
        {
            Response    result  = new Response();
            IEnumerator iter    = WaitForResponse(result, endpoint);

            while (iter.MoveNext()) ;

            return result.isError ? "error" : result.result;
        }

        private IEnumerator WaitForResponse(Response result, string url)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(url);

            yield return    webRequest.SendWebRequest();

            while (!webRequest.isDone) yield return true;

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                result.isError  = true;
                result.result   = webRequest.error;
            }
            else
            {
                result.isError  = false;
                result.result   = webRequest.downloadHandler.text;
            }
        }

        private void SendPost(string url, string json)
        {
            UnityWebRequest postRequest = UnityWebRequest.Post(url, "POST");

            postRequest.uploadHandler   = (UploadHandler) new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(json));
            postRequest.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();

            postRequest.SetRequestHeader("Content-Type", "application/json");
            postRequest.SendWebRequest();
        }

        public static Profile GetProfile()
        {
            if (UserProfile == null)
            {
                UserProfile = new Profile();
            }

            return UserProfile;
        }

        public Difficulty GetDifficulty()
        {
            return taskDifficulty;
        }

        public void SetDifficulty(Difficulty newDifficulty)
        {
            taskDifficulty = newDifficulty;
        }

        public void LoadDifficulty(string levelName)
        {
            currentTask = levelName;
            if (hasInternet)
            {
                string endpoint     = System.String.Format("{0}/difficulty/{1}/{2}", baseURL, id, levelName);
                string jsonContent  = GetRequestAt(endpoint);

                if (!jsonContent.Equals("error"))
                {
                    DifficultyResponse difficultyStr = JsonUtility.FromJson<DifficultyResponse>(jsonContent);

                    switch (difficultyStr.difficulty)
                    {
                        case "Easy":    taskDifficulty = Difficulty.Easy;   break;
                        case "Hard":    taskDifficulty = Difficulty.Hard;   break;
                        case "Medium":  taskDifficulty = Difficulty.Medium; break;
                        default: Debug.Log(difficultyStr.difficulty);       break;
                    }
                }
            }
        }

        public void UpdateDifficulty()
        {
            if (taskDifficulty < Difficulty.Hard && hasInternet)
            {
                taskDifficulty++;

                string url  = System.String.Format("{0}/difficulty/{1}/{2}", baseURL, id, currentTask);
                string json = "{\"difficulty\": " + (int) taskDifficulty + "}";

                SendPost(url, json);
            }
        }

        public void PostEvent(string eventName)
        {
            if (hasInternet)
            {
                string url  = System.String.Format("{0}/event/{1}/{2}", baseURL, id, currentTask);
                string json = "{\"event\": \"" + eventName + "\"}";

                SendPost(url, json);
            }
        }

        public int GetId()
        {
            return id;
        }

        public void SetId(int newId)
        {
            id = newId;
        }

        public bool HasInternetConnection()
        {
            return hasInternet;
        }
    }
}
