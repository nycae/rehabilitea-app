using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace RehabiliTEA
{
    public class Profile
    {
        [System.Serializable]
        public class ProfileData
        {
            public int      id;
            public int      userId;
            public string   title;
            public bool     completed;

            public override string ToString()
            {
                string result   = System.String.Format("id = {0}\n", id);
                result          += System.String.Format("user id = {0}\n", userId);
                result          += System.String.Format("title = {0}\n", title);
                result          += System.String.Format("completed = {0}\n", completed);

                return result;
            }
        }

        private class Response
        {
            public string           result          = "";
        }

        private string              urlEndpoint     = "https://jsonplaceholder.typicode.com/todos/1";
        private ProfileData         data            = null;
        private Difficulty          taskDifficulty  = Difficulty.Hard;

        private static Profile      UserProfile     = null;

        private Profile()
        {
            CreateFromOnlineProfile(1);
        }

        private void CreateFromOnlineProfile(int id)
        {
            Response    result  = new Response();
            IEnumerator iter    = WaitForResponse(result);

            while (iter.MoveNext()) ;

            data = JsonUtility.FromJson<ProfileData>(result.result);
        }

        private IEnumerator WaitForResponse(Response result)
        {
            UnityWebRequest webRequest = UnityWebRequest.Get(urlEndpoint);

            yield return    webRequest.SendWebRequest();

            while (!webRequest.isDone) yield return true;

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                result.result = webRequest.error;
            }
            else
            {
                result.result = webRequest.downloadHandler.text;
            }
        }

        public static Profile GetProfile()
        {
            if (UserProfile == null)
            {
                UserProfile = new Profile();
            }

            return UserProfile;
        }

        public ProfileData GetData()
        {
            return data;
        }

        public Difficulty GetDifficulty()
        {
            return taskDifficulty;
        }
    }
}
