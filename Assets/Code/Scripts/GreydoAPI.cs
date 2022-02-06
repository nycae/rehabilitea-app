using System;
using System.Collections;
using System.Text;
using Unity.UNetWeaver;
using UnityEngine;
using UnityEngine.Networking;

namespace RehabiliTEA
{
    public class UnreachableAPI : Exception {}
        
    public static class GreydoAPI
    {
        private class Response
        {
            public bool   isError; 
            public string result;
        }

        [Serializable] private class JwtRequest
        {
            public string username;
            public string password;
            public JwtRequest(string username, string password)
            {
                this.username = username;
                this.password = password;
            }
        }

        [Serializable] private class JwtResponse
        {
            public string refresh;
            public string access;
        }

        [Serializable] internal class ResponseGame
        {
            public int    id;
            public string name;
            public string description;
        }
       
        [Serializable] internal class ResponseDifficulty
       {
           public int    id;
           public int    order;
           public string name;
       }
       
        [Serializable] private class ProgressionResponse
        {
            public int                id;
            public int                user;
            public ResponseDifficulty difficulty;
            public ResponseGame       game;
        }

        [Serializable] private class UpdateProgressionRequest
        {
            public int user;
            public int game;
            public int difficulty;

            public UpdateProgressionRequest(int user, int difficulty, int game)
            {
                this.user       = user;
                this.game       = game;
                this.difficulty = difficulty;
            }
        }

        [Serializable] private class EventPostRequest
        {
            public string description;
            public int    game;
            public int    user;

            public EventPostRequest(string description, int game, int user)
            {
                this.description = description;
                this.game        = game;
                this.user        = user;
            }
        }

        [Serializable] private class StartGameRequest
        {
            public int user;
            public int game;

            public StartGameRequest(int user, int game)
            {
                this.user = user;
                this.game = game;
            }
        }

        [Serializable] private class StartGameResponse
        {
            public int session_id;
            public int user;
            public int game;
        }

        [Serializable] private class FinishGameRequest
        {
            public int session_id;
            public int score;
            public int max_score;
            public int game;
            public int user;

            public FinishGameRequest(int sessionID, int score, int maxScore, int game, int user)
            {
                this.session_id = sessionID;
                this.score      = score;
                this.max_score  = maxScore;
                this.game       = game;
                this.user       = user;
            }
        }
        
        public static string BaseURL   { get; set; }         = "http://localhost:8000";
        public static string JwtCookie { get; private set; } = "";

        private const string AuthEndpoint        = "auth";
        private const string ProgressionEndpoint = "progression";
        private const string GameIDEndpoint      = "game_id";
        private const string EventsEndpoint      = "events";
        private const string GameStartEndpoint   = "game_start";
        private const string GameEndEndpoint     = "game_end";
        

        public static void RefreshJwt()
        {
            var uri         = $"{ BaseURL }/{ AuthEndpoint }/";
            var credentials = new JwtRequest("user", "pass");
            var response    = SendPostRequest(uri, JsonUtility.ToJson(credentials));

            JwtCookie = !response.isError
                ? JsonUtility.FromJson<JwtResponse>(response.result).access
                : "error";
        }

        public static Difficulty GetDifficulty(int id , string game)
        {
             var endpoint = $"{ BaseURL }/{ ProgressionEndpoint }/{ id }/{ game.ToLower() }/";
             var response = SendGetRequest(endpoint);

             if (response.isError) throw new UnreachableAPI();
             var body     = JsonUtility.FromJson<ProgressionResponse>(response.result);

             return (Difficulty) Enum.Parse(typeof(Difficulty), body.difficulty.name);
        }

        public static int GetGameID(string name)
        {
            var uri      = $"{ BaseURL }/{ GameIDEndpoint }/{ name.ToLower() }";
            var response = SendGetRequest(uri);
            if (response.isError) Debug.Log(response.result);
            return JsonUtility.FromJson<ResponseGame>(response.result).id;
        }

        public static void UpdateDifficulty(int user, int game, int difficulty)
        {
            var uri      = $"{ BaseURL }/{ ProgressionEndpoint }/{ user }/{ game }/";
            var body     = new UpdateProgressionRequest(user, difficulty, game);
            var response = SendPostRequest(uri, JsonUtility.ToJson(body));
            
            if (response.isError) throw new UnreachableAPI();
        }

        public static IEnumerator SendEvent(string eventName, int game, int user)
        {
            var uri      = $"{ BaseURL }/{ EventsEndpoint }/";
            var body     = new EventPostRequest(eventName, game, user);
            
            return SendBackgroundPostRequest(uri, JsonUtility.ToJson(body));
        }

        public static int StartGame(int user, int game)
        {
            var uri      = $"{ BaseURL }/{ GameStartEndpoint }/";
            var body     = new StartGameRequest(user, game);
            var response = SendPostRequest(uri, JsonUtility.ToJson(body));
            
            if (response.isError) throw new UnreachableAPI();

            return JsonUtility.FromJson<StartGameResponse>(response.result).session_id;
        }

        public static IEnumerator PostScore(int sessionID, int score, int maxScore, int game, int user)
        {
            var uri  = $"{ BaseURL }/{ GameEndEndpoint }/";
            var body = new FinishGameRequest(sessionID, score, maxScore, game, user);
            
            return SendBackgroundPostRequest(uri, JsonUtility.ToJson(body));
        }

        private static Response SendPostRequest(string uri, string data)
        {
            var res = new Response();
            using (var webRequest = new UnityWebRequest(uri, "POST"))
            {
                webRequest.uploadHandler   = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Authorization", $"Bearer { JwtCookie }");
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SendWebRequest();
               
                while (!webRequest.isDone) {}

                res.isError = webRequest.isNetworkError || webRequest.isHttpError;
                res.result  = res.isError ? webRequest.error : webRequest.downloadHandler.text;
            }
            return res;
        }

        private static IEnumerator SendBackgroundPostRequest(string uri, string data)
        {
            using (var webRequest = new UnityWebRequest(uri, "POST"))
            {
                webRequest.uploadHandler   = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
                webRequest.downloadHandler = new DownloadHandlerBuffer();
                webRequest.SetRequestHeader("Authorization", $"Bearer { JwtCookie }");
                webRequest.SetRequestHeader("Content-Type", "application/json");
                
                yield return webRequest.SendWebRequest();
               
                while (!webRequest.isDone) {}
            }
        }

        private static Response SendGetRequest(string uri)
        {
            var res = new Response();
            using (var webRequest = UnityWebRequest.Get(uri))
            {
                webRequest.SetRequestHeader("Authorization", $"Bearer { JwtCookie }");
                webRequest.SetRequestHeader("Content-Type", "application/json");
                webRequest.SendWebRequest();
                
                while (!webRequest.isDone) {}

                res.isError = webRequest.isNetworkError || webRequest.isHttpError;
                res.result  = res.isError ? webRequest.error : webRequest.downloadHandler.text;
            }
            return res;
        }
    }
}