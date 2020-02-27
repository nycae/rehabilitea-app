using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Memory.UI
{

public class MemoryScoreUI : MonoBehaviour
{
    [SerializeField]
    private Framework.MemoryGameManager manager;

    [SerializeField]
    private Text currentScoreText;

    [SerializeField]
    private Text targetScoreText;

    void Start()
    {
        targetScoreText.text = manager.GetTargetScore().ToString("00");
    }

    void Update()
    {
        currentScoreText.text = manager.GetCurrentScore().ToString("00");
    }
}

}