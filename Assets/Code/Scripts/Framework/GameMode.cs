using Global.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RehabiliTEA.Framework
{
    public class GameMode : MonoBehaviour
    {
        [SerializeField]
        protected Difficulty        difficulty;

        public delegate void        EndGame(bool wasSuccessfull);
        public static event EndGame OnGameEnd;

        private void Awake()
        {
            this.difficulty = RehabiliTEA.Profile.GetProfile().GetDifficulty();
        }
    }
}