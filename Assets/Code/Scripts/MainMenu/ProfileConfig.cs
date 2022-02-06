using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RehabiliTEA
{
    public class ProfileConfig : MonoBehaviour
    {
        private static void SetDifficulty(Difficulty newDifficulty)
        {
            Profile.GetProfile().TaskDifficulty = newDifficulty;
        }

        public void SetDifficultyEasy()
        {
            SetDifficulty(Difficulty.Easy);
        }

        public void SetDifficultyHard()
        {
            SetDifficulty(Difficulty.Hard);
        }

        public void SetDifficultyMed()
        {
            SetDifficulty(Difficulty.Medium);
        }
    }
}