using System;
using System.IO;
using System.Linq;
using System.Collections;
using UnityEngine;

namespace RehabiliTEA
{
    public class Profile
    {

        [Serializable] private class PersistentData 
        { 
            public int    userID; 
            public string baseURL;

            public PersistentData() {}
            public PersistentData(int userID, string baseURL)
            {
                this.userID  = userID;
                this.baseURL = baseURL;
            }
        }
        
        private readonly string     profileFilePath = Application.persistentDataPath + "/user.json";
        private          int        currentTask;
        private          int        sessionID;
        public           int        ID             { get; set; }
        public           bool       HasInternet    { get; private set; }
        public           Difficulty TaskDifficulty { get; set; } = Difficulty.Hard;
        private static   Profile    _userProfile;

        private Profile()
        {
            if (!File.Exists(profileFilePath)) return;

            var fileContent = File.ReadAllText(profileFilePath);
            var savedData   = JsonUtility.FromJson<PersistentData>(fileContent);
            
            ID                = savedData.userID;
            GreydoAPI.BaseURL = savedData.baseURL;
        }

        ~Profile()
        {
            if (File.Exists(profileFilePath)) File.Delete(profileFilePath);
            
            var file = File.CreateText(profileFilePath);

            file.Write(JsonUtility.ToJson(new PersistentData(ID, GreydoAPI.BaseURL)));
            file.Close();
        }

        public void CheckInternet()
        {
            GreydoAPI.RefreshJwt();
            HasInternet = GreydoAPI.JwtCookie != "error";
        }

        public static Profile GetProfile() { return _userProfile ?? (_userProfile = new Profile()); }
        
        public void LoadDifficulty(string levelName)
        {
             if (!HasInternet) return;

             try
             {
                 TaskDifficulty = GreydoAPI.GetDifficulty(ID, levelName);
                 currentTask    = GreydoAPI.GetGameID(levelName);
                 sessionID      = GreydoAPI.StartGame(ID, currentTask);
             }
             catch (UnreachableAPI)
             {
                 TaskDifficulty = Difficulty.Hard;
             }
        }

        public void UpdateDifficulty()
        {
             if (TaskDifficulty >= Difficulty.Hard || !HasInternet) return;
             
             TaskDifficulty++;
             GreydoAPI.UpdateDifficulty(ID, currentTask, (int) TaskDifficulty);
        }

        public IEnumerator PostEvent(string eventName)
        {
            return HasInternet 
                ? GreydoAPI.SendEvent(eventName, currentTask, ID) 
                : Enumerable.Empty<int>().GetEnumerator();
        }

        public IEnumerator PostScore(int score, int maxScore)
        {
            return HasInternet 
                ? GreydoAPI.PostScore(sessionID, score, maxScore, currentTask, ID)
                : Enumerable.Empty<int>().GetEnumerator();
        }
    }
}
